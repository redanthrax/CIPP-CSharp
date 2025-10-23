using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Models.ODataErrors;

namespace CIPP.Api.Modules.Applications.Services;

public class ApplicationService : IApplicationService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        IMicrosoftGraphService graphService,
        ILogger<ApplicationService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<ApplicationDto>> GetApplicationsAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting applications for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var requestBuilder = graphClient.Applications;

        var response = await requestBuilder.GetAsync(config => {
            config.QueryParameters.Top = paging?.PageSize ?? 100;
            config.QueryParameters.Count = true;
            if (!string.IsNullOrEmpty(paging?.SkipToken)) {
                config.QueryParameters.Skip = int.Parse(paging.SkipToken);
            }
        }, cancellationToken);

        if (response?.Value == null) {
            return new PagedResponse<ApplicationDto> {
                Items = new List<ApplicationDto>(),
                TotalCount = 0,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? 100
            };
        }

        var applications = response.Value.Select(app => MapToApplicationDto(app, tenantId)).ToList();

        return new PagedResponse<ApplicationDto> {
            Items = applications,
            TotalCount = (int)(response.OdataCount ?? applications.Count),
            PageNumber = paging?.PageNumber ?? 1,
            PageSize = paging?.PageSize ?? 100,
            SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
        };
    }

    public async Task<ApplicationDto?> GetApplicationAsync(string tenantId, string applicationId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting application {ApplicationId} for tenant {TenantId}", applicationId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var application = await graphClient.Applications[applicationId].GetAsync(cancellationToken: cancellationToken);
            
            if (application == null) {
                return null;
            }

            return MapToApplicationDto(application, tenantId);
        } catch (ODataError ex) when (ex.ResponseStatusCode == 404) {
            return null;
        }
    }

    public async Task<ApplicationDto> UpdateApplicationAsync(string tenantId, string applicationId, UpdateApplicationDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating application {ApplicationId} for tenant {TenantId}", applicationId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var application = new Application();
        
        if (updateDto.DisplayName != null) {
            application.DisplayName = updateDto.DisplayName;
        }
        
        if (updateDto.Description != null) {
            application.Description = updateDto.Description;
        }
        
        if (updateDto.SignInAudience != null) {
            application.SignInAudience = updateDto.SignInAudience;
        }
        
        if (updateDto.RedirectUris != null) {
            application.Web = new Microsoft.Graph.Beta.Models.WebApplication {
                RedirectUris = updateDto.RedirectUris
            };
        }
        
        if (updateDto.Homepage != null) {
            application.Web ??= new Microsoft.Graph.Beta.Models.WebApplication();
            application.Web.HomePageUrl = updateDto.Homepage;
        }
        
        if (updateDto.LogoutUrl != null) {
            application.Web ??= new Microsoft.Graph.Beta.Models.WebApplication();
            application.Web.LogoutUrl = updateDto.LogoutUrl;
        }
        
        if (updateDto.Tags != null) {
            application.Tags = updateDto.Tags;
        }
        
        if (updateDto.Notes != null) {
            application.Notes = updateDto.Notes;
        }
        
        var updatedApp = await graphClient.Applications[applicationId].PatchAsync(application, cancellationToken: cancellationToken);
        
        if (updatedApp == null) {
            throw new InvalidOperationException("Failed to update application");
        }
        
        return MapToApplicationDto(updatedApp, tenantId);
    }

    public async Task DeleteApplicationAsync(string tenantId, string applicationId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting application {ApplicationId} for tenant {TenantId}", applicationId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        await graphClient.Applications[applicationId].DeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task<ApplicationCredentialDto> CreateApplicationCredentialAsync(CreateApplicationCredentialDto createCredentialDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating credential for application {ApplicationId} in tenant {TenantId}", createCredentialDto.ApplicationId, createCredentialDto.TenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(createCredentialDto.TenantId);
        
        if (createCredentialDto.Type.Equals("Password", StringComparison.OrdinalIgnoreCase)) {
            var passwordCredential = new PasswordCredential {
                DisplayName = createCredentialDto.DisplayName,
                EndDateTime = DateTimeOffset.UtcNow.AddMonths(createCredentialDto.DurationInMonths)
            };

            var result = await graphClient.Applications[createCredentialDto.ApplicationId]
                .AddPassword
                .PostAsync(new Microsoft.Graph.Beta.Applications.Item.AddPassword.AddPasswordPostRequestBody {
                    PasswordCredential = passwordCredential
                }, cancellationToken: cancellationToken);

            if (result == null) {
                throw new InvalidOperationException("Failed to create password credential");
            }

            return new ApplicationCredentialDto {
                KeyId = result.KeyId?.ToString(),
                DisplayName = result.DisplayName,
                StartDateTime = result.StartDateTime?.DateTime,
                EndDateTime = result.EndDateTime?.DateTime,
                Hint = result.Hint,
                Type = "Password"
            };
        } else {
            throw new NotImplementedException("Certificate credentials not yet implemented");
        }
    }

    public async Task DeleteApplicationCredentialAsync(DeleteApplicationCredentialDto deleteCredentialDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting credential {KeyId} for application {ApplicationId} in tenant {TenantId}", deleteCredentialDto.KeyId, deleteCredentialDto.ApplicationId, deleteCredentialDto.TenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(deleteCredentialDto.TenantId);
        
        await graphClient.Applications[deleteCredentialDto.ApplicationId]
            .RemovePassword
            .PostAsync(new Microsoft.Graph.Beta.Applications.Item.RemovePassword.RemovePasswordPostRequestBody {
                KeyId = Guid.Parse(deleteCredentialDto.KeyId)
            }, cancellationToken: cancellationToken);
    }

    private static ApplicationDto MapToApplicationDto(Application application, string tenantId) {
        return new ApplicationDto {
            Id = application.Id ?? string.Empty,
            AppId = application.AppId ?? string.Empty,
            DisplayName = application.DisplayName ?? string.Empty,
            Description = application.Description ?? string.Empty,
            CreatedDateTime = application.CreatedDateTime?.DateTime,
            SignInAudience = application.SignInAudience ?? string.Empty,
            RedirectUris = application.Web?.RedirectUris?.ToList() ?? new List<string>(),
            PublisherDomain = application.PublisherDomain,
            TenantId = tenantId,
            PasswordCredentials = application.PasswordCredentials?.Select(pc => new ApplicationCredentialDto {
                KeyId = pc.KeyId?.ToString(),
                DisplayName = pc.DisplayName,
                StartDateTime = pc.StartDateTime?.DateTime,
                EndDateTime = pc.EndDateTime?.DateTime,
                Hint = pc.Hint,
                Type = "Password"
            }).ToList() ?? new List<ApplicationCredentialDto>(),
            KeyCredentials = application.KeyCredentials?.Select(kc => new ApplicationCredentialDto {
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
