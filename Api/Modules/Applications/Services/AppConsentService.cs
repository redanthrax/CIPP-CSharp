using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Services;

public class AppConsentService : IAppConsentService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<AppConsentService> _logger;

    public AppConsentService(
        IMicrosoftGraphService graphService,
        ILogger<AppConsentService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<AppConsentRequestDto>> GetAppConsentRequestsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting app consent requests for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var response = await graphClient.Oauth2PermissionGrants.GetAsync(config => {
                config.QueryParameters.Top = paging?.PageSize ?? 100;
                config.QueryParameters.Count = true;
                if (!string.IsNullOrEmpty(paging?.SkipToken)) {
                    config.QueryParameters.Skip = int.Parse(paging.SkipToken);
                }
            }, cancellationToken);

            if (response?.Value == null) {
                return new PagedResponse<AppConsentRequestDto> {
                    Items = new List<AppConsentRequestDto>(),
                    TotalCount = 0,
                    PageNumber = paging?.PageNumber ?? 1,
                    PageSize = paging?.PageSize ?? 100
                };
            }

            var consentRequests = new List<AppConsentRequestDto>();
            foreach (var grant in response.Value) {
                var servicePrincipal = await graphClient.ServicePrincipals[grant.ClientId].GetAsync(cancellationToken: cancellationToken);
                
                consentRequests.Add(new AppConsentRequestDto {
                    Id = grant.Id ?? string.Empty,
                    AppId = grant.ClientId ?? string.Empty,
                    AppDisplayName = servicePrincipal?.DisplayName ?? "Unknown",
                    RequestedBy = grant.PrincipalId ?? "Unknown",
                    RequestDateTime = grant.AdditionalData?.ContainsKey("createdDateTime") == true 
                        ? (DateTime?)grant.AdditionalData["createdDateTime"] 
                        : null,
                    Status = grant.ConsentType ?? "Unknown",
                    RequestedPermissions = grant.Scope?.Split(' ').ToList() ?? new List<string>(),
                    TenantId = tenantId
                });
            }

            return new PagedResponse<AppConsentRequestDto> {
                Items = consentRequests,
                TotalCount = (int)(response.OdataCount ?? consentRequests.Count),
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? 100,
                SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving app consent requests for tenant {TenantId}", tenantId);
            return new PagedResponse<AppConsentRequestDto> {
                Items = new List<AppConsentRequestDto>(),
                TotalCount = 0,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? 100
            };
        }
    }

    private static string? ExtractSkipToken(string? nextLink) {
        if (string.IsNullOrEmpty(nextLink)) return null;
        var uri = new Uri(nextLink);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("$skip", out var token) ? token.ToString() : null;
    }
}
