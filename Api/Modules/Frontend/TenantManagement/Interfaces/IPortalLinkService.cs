using CIPP.Api.Modules.Frontend.TenantManagement.Models;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Interfaces;

public interface IPortalLinkService {
    Task<PortalLinks> GeneratePortalLinksAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<PortalLinks> GetCachedPortalLinksAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<string> GetPortalUrlAsync(Guid tenantId, string portalType, CancellationToken cancellationToken = default);
}