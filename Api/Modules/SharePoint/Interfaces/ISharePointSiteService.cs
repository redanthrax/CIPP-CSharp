using CIPP.Shared.DTOs.SharePoint;

namespace CIPP.Api.Modules.SharePoint.Interfaces;

public interface ISharePointSiteService {
    Task<List<SharePointSiteDto>> GetSitesAsync(string tenantId, string type, CancellationToken cancellationToken = default);
    Task<SharePointSiteDto?> GetSiteAsync(string tenantId, string siteId, CancellationToken cancellationToken = default);
    Task<string> CreateSiteAsync(string tenantId, CreateSharePointSiteDto createDto, CancellationToken cancellationToken = default);
    Task DeleteSiteAsync(string tenantId, string siteId, CancellationToken cancellationToken = default);
    Task<SharePointSettingsDto> GetSettingsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<SharePointQuotaDto> GetQuotaAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<string> SetPermissionsAsync(string tenantId, string userId, string accessUser, string? url, bool removePermission, CancellationToken cancellationToken = default);
}
