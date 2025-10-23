using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Tenants.Interfaces;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using DispatchR.Abstractions.Send;
using CIPP.Api.Modules.MsGraph.Interfaces;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, Task<PagedResponse<Tenant>>>
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantCacheService _cacheService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<GetTenantsQueryHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public GetTenantsQueryHandler(
        ApplicationDbContext context, 
        ITenantCacheService cacheService,
        IMicrosoftGraphService graphService,
        ILogger<GetTenantsQueryHandler> logger,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _cacheService = cacheService;
        _graphService = graphService;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    
    public async Task<PagedResponse<Tenant>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var shouldSync = request.NoCache;
            var dbTenantCount = 0;
            
            if (!shouldSync)
            {
                dbTenantCount = await _context.GetEntitySet<Tenant>().CountAsync(cancellationToken);
                shouldSync = dbTenantCount == 0;
            }
            
            if (shouldSync)
            {
                if (dbTenantCount == 0)
                {
                    _logger.LogInformation(
                        "No tenants found in database, automatically syncing from Microsoft Graph");
                }
                
                await SyncPartnerTenantsAsync(cancellationToken);
            }
            
            var skip = (request.PageNumber - 1) * request.PageSize;
            
            if (!shouldSync)
            {
                var cachedTenants = await _cacheService.GetTenantsAsync(request.Filter, skip, request.PageSize);
                var cachedCount = await _cacheService.GetTenantCountAsync(request.Filter);
                
                if (cachedTenants.Any() && cachedCount > 0)
                {
                    _logger.LogDebug("Retrieved {Count} tenants from cache (Page {PageNumber})", 
                        cachedTenants.Count, request.PageNumber);
                    
                    return new PagedResponse<Tenant>
                    {
                        Items = cachedTenants,
                        TotalCount = cachedCount,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    };
                }
            }
            
            var query = _context.GetEntitySet<Tenant>().AsQueryable();
            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(t => 
                    t.DisplayName.Contains(request.Filter) || 
                    t.TenantId.Contains(request.Filter) ||
                    t.DefaultDomainName.Contains(request.Filter));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                query = request.SortBy.ToLowerInvariant() switch
                {
                    "displayname" => request.SortDescending 
                        ? query.OrderByDescending(t => t.DisplayName)
                        : query.OrderBy(t => t.DisplayName),
                    "tenantid" => request.SortDescending 
                        ? query.OrderByDescending(t => t.TenantId)
                        : query.OrderBy(t => t.TenantId),
                    "createdat" => request.SortDescending 
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt),
                    _ => query.OrderBy(t => t.DisplayName)
                };
            }
            else
            {
                query = query.OrderBy(t => t.DisplayName);
            }
            query = query.Skip(skip).Take(request.PageSize);
            var tenants = await query.ToListAsync(cancellationToken);
            
            await _cacheService.SetTenantsListAsync(tenants, request.Filter, skip, request.PageSize);
            await _cacheService.SetTenantCountAsync(totalCount, request.Filter);
            
            _logger.LogInformation(
                "Retrieved {Count} of {TotalCount} tenants from database (Page {PageNumber})", 
                tenants.Count, totalCount, request.PageNumber);
            
            return new PagedResponse<Tenant>
            {
                Items = tenants,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenants");
            throw;
        }
    }

    private async Task SyncPartnerTenantsAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting sync of partner tenants from Microsoft Graph");
            
            var partnerTenantsResponse = await _graphService.GetPartnerTenantsAsync();
            
            if (partnerTenantsResponse?.Value == null)
            {
                _logger.LogWarning(
                    "No partner tenants found or failed to retrieve from Microsoft Graph");
                return;
            }
            
            var partnerTenants = partnerTenantsResponse.Value;
            _logger.LogInformation(
                "Retrieved {Count} partner tenants from Microsoft Graph", partnerTenants.Count);
            
            var tenantsToUpdate = new List<Tenant>();
            
            foreach (var contract in partnerTenants)
            {
                if (contract.CustomerId == null || string.IsNullOrEmpty(contract.DisplayName))
                    continue;
                
                var customerIdString = contract.CustomerId.ToString();
                
                var existingTenant = await _context.GetEntitySet<Tenant>()
                    .FirstOrDefaultAsync(t => t.TenantId == customerIdString, cancellationToken);
                
                if (existingTenant != null)
                {
                    existingTenant.DisplayName = contract.DisplayName;
                    existingTenant.DefaultDomainName = contract.DefaultDomainName ?? 
                        existingTenant.DefaultDomainName;
                    existingTenant.Status = "Active";
                    
                    _context.Update(existingTenant);
                    tenantsToUpdate.Add(existingTenant);
                }
                else
                {
                    var currentUserId = _currentUserService.GetCurrentUserId() ?? Guid.Empty;
                    var newTenant = new Tenant
                    {
                        Id = Guid.NewGuid(),
                        TenantId = customerIdString!,
                        DisplayName = contract.DisplayName,
                        DefaultDomainName = contract.DefaultDomainName ?? 
                            $"{customerIdString}.onmicrosoft.com",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        Metadata = System.Text.Json.JsonSerializer.Serialize(new { 
                            ContractType = contract.ContractType,
                            SyncedFromGraph = true,
                            LastSyncAt = DateTime.UtcNow
                        })
                    };
                    
                    await _context.AddAsync(newTenant, cancellationToken);
                    tenantsToUpdate.Add(newTenant);
                }
            }
            
            var changesCount = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation(
                "Synced {Count} partner tenants to database ({ChangesCount} changes)", 
                tenantsToUpdate.Count, changesCount);
            
            await _cacheService.SetTenantsListAsync(
                tenantsToUpdate, null, 0, tenantsToUpdate.Count);
            await _cacheService.SetTenantCountAsync(tenantsToUpdate.Count, null);
            
            _logger.LogInformation("Successfully synced partner tenants and updated cache");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync partner tenants from Microsoft Graph");
            throw;
        }
    }
}
