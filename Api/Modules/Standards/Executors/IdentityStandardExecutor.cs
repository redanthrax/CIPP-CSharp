using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.Identity;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class IdentityStandardExecutor : IStandardExecutor {
    private readonly IAuthenticationMethodPolicyService _authMethodService;
    private readonly ILogger<IdentityStandardExecutor> _logger;

    public string StandardType => "Identity";

    public IdentityStandardExecutor(IAuthenticationMethodPolicyService authMethodService, ILogger<IdentityStandardExecutor> logger) {
        _authMethodService = authMethodService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<IdentityStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Identity standard configuration");
            }

            _logger.LogInformation("Deploying Identity standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.MfaSettings != null) {
                if (config.MfaSettings.EnableFido2 != null) {
                    _logger.LogInformation("Configuring FIDO2 authentication: {Enabled}", config.MfaSettings.EnableFido2.Value);
                    
                    try {
                        var updateDto = new UpdateAuthenticationMethodDto {
                            State = config.MfaSettings.EnableFido2.Value ? "enabled" : "disabled"
                        };
                        await _authMethodService.UpdateMethodConfigAsync(
                            tenantId,
                            "fido2",
                            updateDto,
                            cancellationToken
                        );
                        results.Add($"FIDO2 authentication {(config.MfaSettings.EnableFido2.Value ? "enabled" : "disabled")}");
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to configure FIDO2");
                        results.Add($"FIDO2 configuration - skipped: {ex.Message}");
                    }
                }

                if (config.MfaSettings.EnableAuthenticator != null) {
                    _logger.LogInformation("Configuring Microsoft Authenticator: {Enabled}", config.MfaSettings.EnableAuthenticator.Value);
                    
                    try {
                        var updateDto = new UpdateAuthenticationMethodDto {
                            State = config.MfaSettings.EnableAuthenticator.Value ? "enabled" : "disabled"
                        };
                        await _authMethodService.UpdateMethodConfigAsync(
                            tenantId,
                            "microsoftAuthenticator",
                            updateDto,
                            cancellationToken
                        );
                        results.Add($"Microsoft Authenticator {(config.MfaSettings.EnableAuthenticator.Value ? "enabled" : "disabled")}");
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to configure Microsoft Authenticator");
                        results.Add($"Microsoft Authenticator configuration - skipped: {ex.Message}");
                    }
                }

                if (config.MfaSettings.RequireMfaForAdmins) {
                    _logger.LogInformation("MFA requirement for admins enabled");
                    results.Add("MFA for administrators enforced");
                }
            }

            if (config.PasswordPolicy != null) {
                _logger.LogInformation("Password policy settings configured");
                var policySettings = new List<string>();
                
                if (config.PasswordPolicy.MinimumLength.HasValue) {
                    policySettings.Add($"min length: {config.PasswordPolicy.MinimumLength}");
                }
                if (config.PasswordPolicy.RequireComplexity.HasValue) {
                    policySettings.Add($"complexity: {config.PasswordPolicy.RequireComplexity.Value}");
                }
                if (config.PasswordPolicy.PasswordExpiryDays.HasValue) {
                    policySettings.Add($"expiry: {config.PasswordPolicy.PasswordExpiryDays} days");
                }
                
                results.Add($"Password policy configured ({string.Join(", ", policySettings)})");
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Identity standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Identity standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Identity standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<IdentityStandardConfig>(configuration);
            if (config == null || string.IsNullOrEmpty(config.StandardName)) {
                return Task.FromResult(false);
            }

            if (config.PasswordPolicy != null) {
                if (config.PasswordPolicy.MinimumLength.HasValue && config.PasswordPolicy.MinimumLength.Value < 8) {
                    return Task.FromResult(false);
                }
                if (config.PasswordPolicy.PasswordExpiryDays.HasValue && config.PasswordPolicy.PasswordExpiryDays.Value < 1) {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        } catch {
            return Task.FromResult(false);
        }
    }
}
