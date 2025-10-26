using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Identity;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Identity.Services;

public class AuthenticationMethodPolicyService : IAuthenticationMethodPolicyService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<AuthenticationMethodPolicyService> _logger;

    public AuthenticationMethodPolicyService(
        IMicrosoftGraphService graphService,
        ILogger<AuthenticationMethodPolicyService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<AuthenticationMethodPolicyDto?> GetPolicyAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting authentication method policy for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policy = await graphClient.Policies.AuthenticationMethodsPolicy.GetAsync(cancellationToken: cancellationToken);

        if (policy == null) {
            return null;
        }

        return new AuthenticationMethodPolicyDto {
            Id = policy.Id ?? string.Empty,
            DisplayName = policy.DisplayName,
            Description = policy.Description,
            AuthenticationMethodConfigurations = policy.AuthenticationMethodConfigurations?
                .Select(c => new AuthenticationMethodConfigurationDto {
                    Id = c.Id ?? string.Empty,
                    State = c.State?.ToString() ?? string.Empty
                }).ToList() ?? new List<AuthenticationMethodConfigurationDto>()
        };
    }

    public async Task<AuthenticationMethodDto?> GetMethodConfigAsync(Guid tenantId, string methodId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting authentication method config {MethodId} for tenant {TenantId}", methodId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var config = await graphClient.Policies.AuthenticationMethodsPolicy.AuthenticationMethodConfigurations[methodId]
            .GetAsync(cancellationToken: cancellationToken);

        if (config == null) {
            return null;
        }

        return MapToAuthenticationMethodDto(config);
    }

    public async Task<AuthenticationMethodDto> UpdateMethodConfigAsync(Guid tenantId, string methodId, UpdateAuthenticationMethodDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating authentication method config {MethodId} for tenant {TenantId}", methodId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var currentConfig = await graphClient.Policies.AuthenticationMethodsPolicy.AuthenticationMethodConfigurations[methodId]
            .GetAsync(cancellationToken: cancellationToken);

        if (currentConfig == null) {
            throw new InvalidOperationException($"Authentication method {methodId} not found");
        }

        ApplyUpdates(currentConfig, updateDto, methodId);

        var updatedConfig = await graphClient.Policies.AuthenticationMethodsPolicy.AuthenticationMethodConfigurations[methodId]
            .PatchAsync(currentConfig, cancellationToken: cancellationToken);

        if (updatedConfig == null) {
            throw new InvalidOperationException($"Failed to update authentication method {methodId}");
        }

        return MapToAuthenticationMethodDto(updatedConfig);
    }

    private static void ApplyUpdates(AuthenticationMethodConfiguration config, UpdateAuthenticationMethodDto updateDto, string methodId) {
        if (!string.IsNullOrEmpty(updateDto.State)) {
            config.State = Enum.Parse<AuthenticationMethodState>(updateDto.State, true);
        }

        switch (methodId.ToUpperInvariant()) {
            case "FIDO2":
                if (config is Fido2AuthenticationMethodConfiguration fido2Config) {
                    if (updateDto.IsAttestationEnforced.HasValue) {
                        fido2Config.IsAttestationEnforced = updateDto.IsAttestationEnforced.Value;
                    }
                    if (updateDto.IsSelfServiceRegistrationAllowed.HasValue) {
                        fido2Config.IsSelfServiceRegistrationAllowed = updateDto.IsSelfServiceRegistrationAllowed.Value;
                    }
                }
                break;

            case "MICROSOFTAUTHENTICATOR":
                if (config is MicrosoftAuthenticatorAuthenticationMethodConfiguration msAuthConfig) {
                    if (updateDto.IsSoftwareOathEnabled.HasValue) {
                        msAuthConfig.IsSoftwareOathEnabled = updateDto.IsSoftwareOathEnabled.Value;
                    }
                }
                break;

            case "TEMPORARYACCESSPASS":
                if (config is TemporaryAccessPassAuthenticationMethodConfiguration tapConfig) {
                    if (updateDto.MinimumLifetimeInMinutes.HasValue) {
                        tapConfig.MinimumLifetimeInMinutes = updateDto.MinimumLifetimeInMinutes.Value;
                    }
                    if (updateDto.MaximumLifetimeInMinutes.HasValue) {
                        tapConfig.MaximumLifetimeInMinutes = updateDto.MaximumLifetimeInMinutes.Value;
                    }
                    if (updateDto.DefaultLifetimeInMinutes.HasValue) {
                        tapConfig.DefaultLifetimeInMinutes = updateDto.DefaultLifetimeInMinutes.Value;
                    }
                    if (updateDto.DefaultLength.HasValue) {
                        tapConfig.DefaultLength = updateDto.DefaultLength.Value;
                    }
                    if (updateDto.IsUsableOnce.HasValue) {
                        tapConfig.IsUsableOnce = updateDto.IsUsableOnce.Value;
                    }
                }
                break;
        }
    }

    private static AuthenticationMethodDto MapToAuthenticationMethodDto(AuthenticationMethodConfiguration config) {
        return new AuthenticationMethodDto {
            Id = config.Id ?? string.Empty,
            State = config.State?.ToString() ?? string.Empty
        };
    }
}
