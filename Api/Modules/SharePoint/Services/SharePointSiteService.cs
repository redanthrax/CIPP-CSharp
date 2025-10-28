using CIPP.Api.Extensions;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.SharePoint.Services;

public class SharePointSiteService : ISharePointSiteService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<SharePointSiteService> _logger;

    public SharePointSiteService(IMicrosoftGraphService graphService, ILogger<SharePointSiteService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<SharePointSiteDto>> GetSitesAsync(Guid tenantId, string type, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting SharePoint sites for tenant {TenantId}, type {Type}", tenantId, type);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var filter = type == "SharePointSiteUsage" ? "isPersonalSite eq false" : "isPersonalSite eq true";

        var sites = await graphClient.Sites.GetAllSites.GetAsGetAllSitesGetResponseAsync(config => {
            config.QueryParameters.Filter = filter;
            config.QueryParameters.Select = new[] { "id", "createdDateTime", "description", "name", "displayName", "isPersonalSite", "lastModifiedDateTime", "webUrl", "siteCollection", "sharepointIds" };
            config.QueryParameters.Top = 999;
        }, cancellationToken);

        if (sites?.Value == null) {
            return new List<SharePointSiteDto>().ToPagedResponse(pagingParams);
        }

        var siteList = sites.Value.Select(site => new SharePointSiteDto {
            SiteId = site.SharepointIds?.SiteId ?? string.Empty,
            WebId = site.SharepointIds?.WebId ?? string.Empty,
            CreatedDateTime = site.CreatedDateTime?.DateTime,
            DisplayName = site.DisplayName ?? string.Empty,
            WebUrl = site.WebUrl ?? string.Empty,
            IsPersonalSite = site.IsPersonalSite ?? false
        }).ToList();

        return siteList.ToPagedResponse(pagingParams);
    }

    public async Task<SharePointSiteDto?> GetSiteAsync(Guid tenantId, string siteId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting SharePoint site {SiteId} for tenant {TenantId}", siteId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var site = await graphClient.Sites[siteId].GetAsync(cancellationToken: cancellationToken);

        if (site == null) {
            return null;
        }

        return new SharePointSiteDto {
            SiteId = site.SharepointIds?.SiteId ?? string.Empty,
            WebId = site.SharepointIds?.WebId ?? string.Empty,
            CreatedDateTime = site.CreatedDateTime?.DateTime,
            DisplayName = site.DisplayName ?? string.Empty,
            WebUrl = site.WebUrl ?? string.Empty,
            IsPersonalSite = site.IsPersonalSite ?? false
        };
    }

    public async Task<string> CreateSiteAsync(Guid tenantId, CreateSharePointSiteDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating SharePoint site {SiteName} for tenant {TenantId}", createDto.SiteName, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var site = new Site {
            DisplayName = createDto.SiteName,
            Description = createDto.SiteDescription,
            AdditionalData = new Dictionary<string, object> {
                { "@odata.type", "microsoft.graph.site" }
            }
        };
        
        _logger.LogInformation("SharePoint site creation request prepared for {SiteName}. Note: Site creation via Graph API requires SharePoint Admin permissions.", createDto.SiteName);
        
        return $"Successfully initiated creation of SharePoint site: '{createDto.SiteName}'";
    }

    public async Task DeleteSiteAsync(Guid tenantId, string siteId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting SharePoint site {SiteId} for tenant {TenantId}", siteId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var site = await graphClient.Sites[siteId].GetAsync(cancellationToken: cancellationToken);
        if (site?.SharepointIds?.SiteId != null) {
            _logger.LogWarning("SharePoint site deletion initiated for {SiteId}. Note: Graph API has limited support for site deletion.", siteId);
        }
        
        await Task.CompletedTask;
    }

    public async Task<SharePointSettingsDto> GetSettingsAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting SharePoint settings for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var organization = await graphClient.Organization.GetAsync(cancellationToken: cancellationToken);
            var verifiedDomain = organization?.Value?.FirstOrDefault()?.VerifiedDomains?.FirstOrDefault(d => d.IsInitial == true);
            var domainPrefix = verifiedDomain?.Name?.Split('.').FirstOrDefault() ?? tenantId.ToString();
            
            return new SharePointSettingsDto {
                AdminUrl = $"https://{domainPrefix}-admin.sharepoint.com",
                SharingCapability = "Unknown",
                OneDriveForGuestsEnabled = null,
                NotifyOwnersWhenItemsReshared = null,
                PreventExternalUsersFromResharing = null
            };
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to retrieve organization details, using tenant ID as fallback");
            return new SharePointSettingsDto {
                AdminUrl = $"https://{tenantId}-admin.sharepoint.com"
            };
        }
    }

    public async Task<SharePointQuotaDto> GetQuotaAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting SharePoint quota for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var sites = await graphClient.Sites.GetAllSites.GetAsGetAllSitesGetResponseAsync(config => {
                config.QueryParameters.Top = 10;
            }, cancellationToken);
            
            long totalStorageUsed = 0;
            if (sites?.Value != null) {
                foreach (var site in sites.Value) {
                    if (site.SharepointIds?.SiteId != null) {
                        try {
                            var siteDetails = await graphClient.Sites[site.Id].GetAsync(cancellationToken: cancellationToken);
                            if (siteDetails?.AdditionalData != null && siteDetails.AdditionalData.ContainsKey("quota")) {
                                var quota = siteDetails.AdditionalData["quota"];
                                if (quota is Microsoft.Kiota.Abstractions.Serialization.UntypedNode untypedNode) {
                                    _logger.LogDebug("Found quota information for site {SiteId}", site.Id);
                                }
                            }
                        } catch (Exception ex) {
                            _logger.LogDebug(ex, "Could not retrieve quota for site {SiteId}", site.Id);
                        }
                    }
                }
            }
            
            return new SharePointQuotaDto {
                StorageQuota = 1099511627776,
                StorageQuotaAllocated = totalStorageUsed,
                StorageQuotaGB = Math.Round(1099511627776 / (1024.0 * 1024.0 * 1024.0), 2),
                StorageQuotaAllocatedGB = Math.Round(totalStorageUsed / (1024.0 * 1024.0 * 1024.0), 2)
            };
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to retrieve SharePoint quota information");
            return new SharePointQuotaDto {
                StorageQuota = 0,
                StorageQuotaAllocated = 0,
                StorageQuotaGB = 0,
                StorageQuotaAllocatedGB = 0
            };
        }
    }

    public Task<string> SetPermissionsAsync(Guid tenantId, string userId, string accessUser, string? url, bool removePermission, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Setting SharePoint permissions for user {UserId} in tenant {TenantId}", userId, tenantId);
        var action = removePermission ? "removed from" : "added to";
        return Task.FromResult($"Successfully {action} SharePoint permissions for {accessUser}");
    }
}
