using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Queries;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class GetTenantCapabilitiesQueryHandler : IRequestHandler<GetTenantCapabilitiesQuery, Task<TenantCapabilities?>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetTenantCapabilitiesQueryHandler> _logger;

    public GetTenantCapabilitiesQueryHandler(ApplicationDbContext context, ILogger<GetTenantCapabilitiesQueryHandler> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<TenantCapabilities?> Handle(GetTenantCapabilitiesQuery request, CancellationToken cancellationToken) {
        try {
            var tenant = await _context.GetEntitySet<Tenant>()
                .FirstOrDefaultAsync(t => t.TenantId == request.TenantId, cancellationToken);

            if (tenant?.Capabilities == null) {
                _logger.LogWarning("Tenant {TenantId} not found or has no capabilities", request.TenantId);
                return null;
            }

            return tenant.Capabilities;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving capabilities for tenant {TenantId}", request.TenantId);
            throw;
        }
    }
}