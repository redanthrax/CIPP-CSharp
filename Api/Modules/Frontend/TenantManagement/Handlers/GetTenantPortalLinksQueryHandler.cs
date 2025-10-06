using CIPP.Api.Data;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Handlers;

public class GetTenantPortalLinksQueryHandler : IRequestHandler<GetTenantPortalLinksQuery, Task<PortalLinks>> {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetTenantPortalLinksQueryHandler> _logger;

    public GetTenantPortalLinksQueryHandler(ApplicationDbContext context, ILogger<GetTenantPortalLinksQueryHandler> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<PortalLinks> Handle(GetTenantPortalLinksQuery request, CancellationToken cancellationToken) {
        try {
            var tenant = await _context.GetEntitySet<Tenant>()
                .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

            if (tenant == null) {
                throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
            }

            var portalLinks = GeneratePortalLinks(tenant);
            
            _logger.LogDebug("Generated portal links for tenant {TenantId}", tenant.TenantId);
            return portalLinks;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error generating portal links for tenant {TenantId}", request.TenantId);
            throw;
        }
    }

    private static PortalLinks GeneratePortalLinks(Tenant tenant) {
        var tenantId = tenant.TenantId;
        var domain = tenant.DefaultDomainName;
        var domainPrefix = domain.Split('.')[0];

        return new PortalLinks {
            Microsoft365 = $"https://admin.microsoft.com/?tenantId={tenantId}",
            Exchange = $"https://admin.exchange.microsoft.com/?tenantId={tenantId}",
            Entra = $"https://entra.microsoft.com/{domain}",
            Teams = $"https://admin.teams.microsoft.com/?tenantId={tenantId}",
            Azure = $"https://portal.azure.com/{tenantId}",
            Intune = $"https://endpoint.microsoft.com/?tenantId={tenantId}",
            Security = $"https://security.microsoft.com/?tenantId={tenantId}",
            Compliance = $"https://compliance.microsoft.com/?tenantId={tenantId}",
            SharePoint = $"https://{domainPrefix}-admin.sharepoint.com",
            PowerPlatform = $"https://admin.powerplatform.microsoft.com/environments?tenantId={tenantId}",
            PowerBI = $"https://app.powerbi.com/?tenantId={tenantId}"
        };
    }
}