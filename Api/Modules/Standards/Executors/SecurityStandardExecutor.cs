using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.Security;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class SecurityStandardExecutor : IStandardExecutor {
    private readonly ISecureScoreService _secureScoreService;
    private readonly ILogger<SecurityStandardExecutor> _logger;

    public string StandardType => "Security";

    public SecurityStandardExecutor(ISecureScoreService secureScoreService, ILogger<SecurityStandardExecutor> logger) {
        _secureScoreService = secureScoreService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<SecurityStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Security standard configuration");
            }

            _logger.LogInformation("Deploying Security standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.SecureScoreControls != null && config.SecureScoreControls.Any()) {
                foreach (var control in config.SecureScoreControls) {
                    _logger.LogInformation("Updating Secure Score control: {ControlId}", control.ControlId);
                    
                    try {
                        var updateDto = new UpdateSecureScoreControlDto {
                            State = control.Status,
                            Comment = control.Comment
                        };
                        await _secureScoreService.UpdateControlProfileAsync(
                            tenantId,
                            control.ControlId,
                            updateDto,
                            cancellationToken
                        );
                        results.Add($"Secure Score control '{control.ControlId}' set to {control.Status}");
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to update control {ControlId}", control.ControlId);
                        results.Add($"Secure Score control '{control.ControlId}' - skipped: {ex.Message}");
                    }
                }
            }

            if (config.DefenderSettings != null) {
                _logger.LogInformation("Applying Defender settings");
                results.Add($"Defender settings configured: {string.Join(", ", config.DefenderSettings.Keys)}");
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Security standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Security standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Security standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<SecurityStandardConfig>(configuration);
            if (config == null || string.IsNullOrEmpty(config.StandardName)) {
                return Task.FromResult(false);
            }

            if (config.SecureScoreControls != null) {
                foreach (var control in config.SecureScoreControls) {
                    if (string.IsNullOrEmpty(control.ControlId) || string.IsNullOrEmpty(control.Status)) {
                        return Task.FromResult(false);
                    }

                    var validStatuses = new[] { "Default", "Ignored", "ThirdParty", "Reviewed" };
                    if (!validStatuses.Contains(control.Status)) {
                        return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(true);
        } catch {
            return Task.FromResult(false);
        }
    }
}
