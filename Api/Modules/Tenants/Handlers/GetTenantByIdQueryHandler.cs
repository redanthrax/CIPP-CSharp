using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Interfaces;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Queries;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
namespace CIPP.Api.Modules.Tenants.Handlers;
public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, Task<Tenant?>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetTenantByIdQueryHandler> _logger;
    private readonly ITenantCacheService _cacheService;
    public GetTenantByIdQueryHandler(ApplicationDbContext context, ILogger<GetTenantByIdQueryHandler> logger, ITenantCacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }
    public async Task<Tenant?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cachedTenant = await _cacheService.GetTenantAsync(request.Id);
            if (cachedTenant != null)
            {
                _logger.LogInformation("Retrieved tenant {TenantId} with ID {Id} from cache", cachedTenant.TenantId, cachedTenant.Id);
                return cachedTenant;
            }
            var tenant = await _context.GetEntitySet<Tenant>()
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tenant != null)
            {
                await _cacheService.SetTenantAsync(tenant);
                _logger.LogInformation("Retrieved tenant {TenantId} with ID {Id} from database and cached it", tenant.TenantId, tenant.Id);
            }
            else
            {
                _logger.LogWarning("Tenant with ID {Id} not found", request.Id);
            }
            return tenant;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant with ID {Id}", request.Id);
            throw;
        }
    }
}
