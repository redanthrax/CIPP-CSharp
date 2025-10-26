using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Models.ODataErrors;

namespace CIPP.Api.Modules.Applications.Services;

public class ServicePrincipalService : IServicePrincipalService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<ServicePrincipalService> _logger;

    public ServicePrincipalService(
        IMicrosoftGraphService graphService,
        ILogger<ServicePrincipalService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<ServicePrincipalDto>> GetServicePrincipalsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting service principals for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var response = await graphClient.ServicePrincipals.GetAsync(config => {
            config.QueryParameters.Top = paging?.PageSize ?? 100;
            config.QueryParameters.Count = true;
            if (!string.IsNullOrEmpty(paging?.SkipToken)) {
                config.QueryParameters.Skip = int.Parse(paging.SkipToken);
            }
        }, cancellationToken);

        if (response?.Value == null) {
            return new PagedResponse<ServicePrincipalDto> {
                Items = new List<ServicePrincipalDto>(),
                TotalCount = 0,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? 100
            };
        }

        var servicePrincipals = response.Value.Select(sp => MapToServicePrincipalDto(sp, tenantId)).ToList();

        return new PagedResponse<ServicePrincipalDto> {
            Items = servicePrincipals,
            TotalCount = (int)(response.OdataCount ?? servicePrincipals.Count),
            PageNumber = paging?.PageNumber ?? 1,
            PageSize = paging?.PageSize ?? 100,
            SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
        };
    }

    public async Task<ServicePrincipalDto?> GetServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting service principal {ServicePrincipalId} for tenant {TenantId}", servicePrincipalId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var servicePrincipal = await graphClient.ServicePrincipals[servicePrincipalId].GetAsync(cancellationToken: cancellationToken);
            
            if (servicePrincipal == null) {
                return null;
            }

            return MapToServicePrincipalDto(servicePrincipal, tenantId);
        } catch (ODataError ex) when (ex.ResponseStatusCode == 404) {
            return null;
        }
    }

    public async Task<ServicePrincipalDto> UpdateServicePrincipalAsync(Guid tenantId, string servicePrincipalId, UpdateServicePrincipalDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating service principal {ServicePrincipalId} for tenant {TenantId}", servicePrincipalId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var servicePrincipal = new ServicePrincipal();
        
        if (updateDto.DisplayName != null) {
            servicePrincipal.DisplayName = updateDto.DisplayName;
        }
        
        if (updateDto.Description != null) {
            servicePrincipal.Description = updateDto.Description;
        }
        
        if (updateDto.AccountEnabled.HasValue) {
            servicePrincipal.AccountEnabled = updateDto.AccountEnabled.Value;
        }
        
        if (updateDto.Homepage != null) {
            servicePrincipal.Homepage = updateDto.Homepage;
        }
        
        if (updateDto.LogoutUrl != null) {
            servicePrincipal.LogoutUrl = updateDto.LogoutUrl;
        }
        
        if (updateDto.Tags != null) {
            servicePrincipal.Tags = updateDto.Tags;
        }
        
        if (updateDto.Notes != null) {
            servicePrincipal.Notes = updateDto.Notes;
        }
        
        if (updateDto.PreferredSingleSignOnMode != null) {
            servicePrincipal.PreferredSingleSignOnMode = updateDto.PreferredSingleSignOnMode;
        }
        
        var updatedSp = await graphClient.ServicePrincipals[servicePrincipalId].PatchAsync(servicePrincipal, cancellationToken: cancellationToken);
        
        if (updatedSp == null) {
            throw new InvalidOperationException("Failed to update service principal");
        }
        
        return MapToServicePrincipalDto(updatedSp, tenantId);
    }

    public async Task DeleteServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting service principal {ServicePrincipalId} for tenant {TenantId}", servicePrincipalId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        await graphClient.ServicePrincipals[servicePrincipalId].DeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task EnableServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Enabling service principal {ServicePrincipalId} for tenant {TenantId}", servicePrincipalId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var servicePrincipal = new ServicePrincipal {
            AccountEnabled = true
        };

        await graphClient.ServicePrincipals[servicePrincipalId].PatchAsync(servicePrincipal, cancellationToken: cancellationToken);
    }

    public async Task DisableServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Disabling service principal {ServicePrincipalId} for tenant {TenantId}", servicePrincipalId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var servicePrincipal = new ServicePrincipal {
            AccountEnabled = false
        };

        await graphClient.ServicePrincipals[servicePrincipalId].PatchAsync(servicePrincipal, cancellationToken: cancellationToken);
    }

    private static ServicePrincipalDto MapToServicePrincipalDto(ServicePrincipal servicePrincipal, Guid tenantId) {
        return new ServicePrincipalDto {
            Id = servicePrincipal.Id ?? string.Empty,
            AppId = servicePrincipal.AppId ?? string.Empty,
            DisplayName = servicePrincipal.DisplayName ?? string.Empty,
            Description = servicePrincipal.Description,
            AccountEnabled = servicePrincipal.AccountEnabled ?? false,
            CreatedDateTime = servicePrincipal.AdditionalData?.ContainsKey("createdDateTime") == true 
                ? (DateTime?)servicePrincipal.AdditionalData["createdDateTime"] 
                : null,
            ServicePrincipalType = servicePrincipal.ServicePrincipalType ?? string.Empty,
            ServicePrincipalNames = servicePrincipal.ServicePrincipalNames?.ToList() ?? new List<string>(),
            PublisherName = servicePrincipal.PublisherName,
            HomePage = servicePrincipal.Homepage,
            TenantId = tenantId,
            ReplyUrls = servicePrincipal.ReplyUrls?.ToList() ?? new List<string>(),
            PasswordCredentials = servicePrincipal.PasswordCredentials?.Select(pc => new ApplicationCredentialDto {
                KeyId = pc.KeyId?.ToString(),
                DisplayName = pc.DisplayName,
                StartDateTime = pc.StartDateTime?.DateTime,
                EndDateTime = pc.EndDateTime?.DateTime,
                Hint = pc.Hint,
                Type = "Password"
            }).ToList() ?? new List<ApplicationCredentialDto>(),
            KeyCredentials = servicePrincipal.KeyCredentials?.Select(kc => new ApplicationCredentialDto {
                KeyId = kc.KeyId?.ToString(),
                DisplayName = kc.DisplayName,
                StartDateTime = kc.StartDateTime?.DateTime,
                EndDateTime = kc.EndDateTime?.DateTime,
                Type = "Certificate",
                Usage = kc.Usage
            }).ToList() ?? new List<ApplicationCredentialDto>()
        };
    }

    private static string? ExtractSkipToken(string? nextLink) {
        if (string.IsNullOrEmpty(nextLink)) return null;
        var uri = new Uri(nextLink);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("$skip", out var token) ? token.ToString() : null;
    }
}
