using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.Intune;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class IntuneStandardExecutor : IStandardExecutor {
    private readonly IIntunePolicyService _policyService;
    private readonly ILogger<IntuneStandardExecutor> _logger;

    public string StandardType => "Intune";

    public IntuneStandardExecutor(IIntunePolicyService policyService, ILogger<IntuneStandardExecutor> logger) {
        _policyService = policyService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<IntuneStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Intune standard configuration");
            }

            _logger.LogInformation("Deploying Intune standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.DevicePolicies != null && config.DevicePolicies.Any()) {
                foreach (var policy in config.DevicePolicies) {
                    _logger.LogInformation("Creating device policy: {PolicyName}", policy.DisplayName);
                    
                    var createDto = new CreateIntunePolicyDto {
                        DisplayName = policy.DisplayName,
                        Description = policy.Description
                    };

                    await _policyService.CreatePolicyAsync(tenantId, createDto, cancellationToken);
                    results.Add($"Device policy '{policy.DisplayName}' created");
                }
            }

            if (config.CompliancePolicies != null && config.CompliancePolicies.Any()) {
                foreach (var policy in config.CompliancePolicies) {
                    _logger.LogInformation("Creating compliance policy: {PolicyName}", policy.DisplayName);
                    
                    var createDto = new CreateIntunePolicyDto {
                        DisplayName = policy.DisplayName,
                        Description = policy.Description
                    };

                    await _policyService.CreatePolicyAsync(tenantId, createDto, cancellationToken);
                    results.Add($"Compliance policy '{policy.DisplayName}' created");
                }
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Intune standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Intune standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Intune standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<IntuneStandardConfig>(configuration);
            if (config == null || string.IsNullOrEmpty(config.StandardName)) {
                return Task.FromResult(false);
            }

            if (config.DevicePolicies != null) {
                foreach (var policy in config.DevicePolicies) {
                    if (string.IsNullOrEmpty(policy.DisplayName) || string.IsNullOrEmpty(policy.PolicyType)) {
                        return Task.FromResult(false);
                    }
                }
            }

            if (config.CompliancePolicies != null) {
                foreach (var policy in config.CompliancePolicies) {
                    if (string.IsNullOrEmpty(policy.DisplayName)) {
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
