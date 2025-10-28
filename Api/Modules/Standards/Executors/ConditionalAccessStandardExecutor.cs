using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.ConditionalAccess;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class ConditionalAccessStandardExecutor : IStandardExecutor {
    private readonly IConditionalAccessPolicyService _caService;
    private readonly ILogger<ConditionalAccessStandardExecutor> _logger;

    public string StandardType => "ConditionalAccess";

    public ConditionalAccessStandardExecutor(IConditionalAccessPolicyService caService, ILogger<ConditionalAccessStandardExecutor> logger) {
        _caService = caService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<ConditionalAccessPolicyDto>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Conditional Access policy configuration");
            }

            _logger.LogInformation("Deploying Conditional Access policy {PolicyName} to tenant {TenantId}", config.DisplayName, tenantId);

            var existingPoliciesResponse = await _caService.GetPoliciesAsync(tenantId, null, cancellationToken);
            var existingPolicy = existingPoliciesResponse.Items.FirstOrDefault(p => p.DisplayName == config.DisplayName);

            if (existingPolicy != null) {
                _logger.LogInformation("Updating existing Conditional Access policy {PolicyId}", existingPolicy.Id);
                var updateDto = new UpdateConditionalAccessPolicyDto {
                    DisplayName = config.DisplayName,
                    State = config.State,
                    Conditions = config.Conditions,
                    GrantControls = config.GrantControls,
                    SessionControls = config.SessionControls
                };
                await _caService.UpdatePolicyAsync(tenantId, existingPolicy.Id, updateDto, cancellationToken);
            } else {
                _logger.LogInformation("Creating new Conditional Access policy");
                var createDto = new CreateConditionalAccessPolicyDto {
                    DisplayName = config.DisplayName,
                    State = config.State,
                    Conditions = config.Conditions,
                    GrantControls = config.GrantControls,
                    SessionControls = config.SessionControls
                };
                await _caService.CreatePolicyAsync(createDto, cancellationToken);
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Conditional Access policy '{config.DisplayName}' applied successfully",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Conditional Access standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Conditional Access policy: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<ConditionalAccessPolicyDto>(configuration);
            return Task.FromResult(config != null && !string.IsNullOrEmpty(config.DisplayName));
        } catch {
            return Task.FromResult(false);
        }
    }
}
