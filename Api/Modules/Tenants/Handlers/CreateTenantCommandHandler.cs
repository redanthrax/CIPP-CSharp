using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Interfaces;
using DispatchR.Abstractions.Send;
namespace CIPP.Api.Modules.Tenants.Handlers;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Task<Tenant>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateTenantCommandHandler> _logger;
    private readonly ITenantCacheService _cacheService;
    public CreateTenantCommandHandler(ApplicationDbContext context, ILogger<CreateTenantCommandHandler> logger, ITenantCacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }
    public async Task<Tenant> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenant = new Tenant
            {
                TenantId = request.TenantId,
                DisplayName = request.DisplayName,
                DefaultDomainName = request.DefaultDomainName,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                Metadata = request.Metadata
            };
            _context.GetEntitySet<Tenant>().Add(tenant);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.SetTenantAsync(tenant);
            await _cacheService.InvalidateTenantsListCache();
            _logger.LogInformation("Created tenant {TenantId} and updated cache", tenant.TenantId);
            return tenant;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant {TenantId}", request.TenantId);
            throw;
        }
    }
}
