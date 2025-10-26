using CIPP.Shared.DTOs.SharePoint;

namespace CIPP.Api.Modules.SharePoint.Interfaces;

public interface ISharePointSiteService {
    Task<List<SharePointSiteDto>> GetSitesAsync(Guid tenantId, string type, CancellationToken cancellationToken = default);
    Task<SharePointSiteDto?> GetSiteAsync(Guid tenantId, string siteId, CancellationToken cancellationToken = default);
    Task<string> CreateSiteAsync(Guid tenantId, CreateSharePointSiteDto createDto, CancellationToken cancellationToken = default);
    Task DeleteSiteAsync(Guid tenantId, string siteId, CancellationToken cancellationToken = default);
    Task<SharePointSettingsDto> GetSettingsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<SharePointQuotaDto> GetQuotaAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<string> SetPermissionsAsync(Guid tenantId, string userId, string accessUser, string? url, bool removePermission, CancellationToken cancellationToken = default);
}
