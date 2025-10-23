using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Interfaces;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
using CIPP.Api.Modules.MsGraph.Interfaces;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class SyncTenantFromGraphCommandHandler : IRequestHandler<SyncTenantFromGraphCommand, Task<string>> {
    private readonly ApplicationDbContext _context;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ITenantCacheService _cacheService;
    private readonly ILogger<SyncTenantFromGraphCommandHandler> _logger;

    public SyncTenantFromGraphCommandHandler(
        ApplicationDbContext context,
        IMicrosoftGraphService graphService,
        ITenantCacheService cacheService,
        ILogger<SyncTenantFromGraphCommandHandler> logger) {
        _context = context;
        _graphService = graphService;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<string> Handle(SyncTenantFromGraphCommand request, CancellationToken cancellationToken) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant == null) {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
        }

        try {
            _logger.LogInformation("Starting sync for tenant {TenantId} ({DisplayName}) from Microsoft Graph",
                tenant.TenantId, tenant.DisplayName);

            var organization = await _graphService.GetOrganizationAsync(tenant.TenantId);
            
            tenant.DisplayName = organization?.DisplayName ?? tenant.DisplayName;
            
            if (organization?.VerifiedDomains?.Any() == true) {
                var defaultDomain = organization.VerifiedDomains.FirstOrDefault(d => d.IsDefault == true);
                if (defaultDomain?.Name != null) {
                    tenant.DefaultDomainName = defaultDomain.Name;
                }

                var initialDomain = organization.VerifiedDomains.FirstOrDefault(d => d.IsInitial == true);
                if (initialDomain?.Name != null) {
                    tenant.InitialDomainName = initialDomain.Name;
                }

                tenant.DomainList = organization.VerifiedDomains
                    .Where(d => !string.IsNullOrEmpty(d.Name))
                    .Select(d => d.Name!)
                    .ToList();
            }

            if (organization?.AssignedPlans?.Any() == true) {
                var licenses = organization.AssignedPlans
                    .Where(p => p.ServicePlanId != null && !string.IsNullOrEmpty(p.ServicePlanId.ToString()))
                    .Select(p => p.ServicePlanId!.ToString()!)
                    .Distinct()
                    .ToList();

                if (tenant.Capabilities == null) {
                    tenant.Capabilities = new TenantCapabilities();
                }

                tenant.Capabilities.Licenses = licenses;
                tenant.Capabilities.HasExchange = licenses.Any(l => l?.Contains("EXCHANGE") == true);
                tenant.Capabilities.HasSharePoint = licenses.Any(l => l?.Contains("SHAREPOINT") == true);
                tenant.Capabilities.HasTeams = licenses.Any(l => l?.Contains("TEAMS") == true);
                tenant.Capabilities.HasIntune = licenses.Any(l => l?.Contains("INTUNE") == true);
                tenant.Capabilities.HasDefender = licenses.Any(l => l?.Contains("DEFENDER") == true || l?.Contains("ATP") == true);
            }

            tenant.LastSyncAt = DateTime.UtcNow;
            tenant.GraphErrorCount = 0;

            await _context.SaveChangesAsync(cancellationToken);

            await _cacheService.SetTenantAsync(tenant);
            await _cacheService.InvalidateTenantsListCache();

            _logger.LogInformation("Successfully synced tenant {TenantId} from Microsoft Graph", tenant.TenantId);
            return $"Successfully synced tenant {tenant.DisplayName} from Microsoft Graph";
        }
        catch (Exception ex) when (!(ex is InvalidOperationException)) {
            tenant.GraphErrorCount++;
            tenant.LastSyncAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogError(ex, "Failed to sync tenant {TenantId} ({DisplayName}) from Microsoft Graph", 
                tenant.TenantId, tenant.DisplayName);
            
            throw new InvalidOperationException(
                $"Failed to sync tenant {tenant.DisplayName} from Microsoft Graph: {ex.Message}", ex);
        }
    }
}