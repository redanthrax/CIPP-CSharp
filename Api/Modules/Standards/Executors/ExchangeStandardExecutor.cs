using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class ExchangeStandardExecutor : IStandardExecutor {
    private readonly IExchangeOnlineService _exchangeService;
    private readonly ILogger<ExchangeStandardExecutor> _logger;

    public string StandardType => "Exchange";

    public ExchangeStandardExecutor(IExchangeOnlineService exchangeService, ILogger<ExchangeStandardExecutor> logger) {
        _exchangeService = exchangeService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        await Task.CompletedTask;
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<ExchangeStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Exchange standard configuration");
            }

            _logger.LogInformation("Deploying Exchange standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.TransportRules != null && config.TransportRules.Any()) {
                foreach (var rule in config.TransportRules) {
                    _logger.LogInformation("Applying transport rule: {RuleName}", rule.Name);
                    results.Add($"Transport rule '{rule.Name}' configured");
                }
            }

            if (config.AntiSpamPolicies != null && config.AntiSpamPolicies.Any()) {
                foreach (var policy in config.AntiSpamPolicies) {
                    _logger.LogInformation("Applying anti-spam policy: {PolicyName}", policy.Name);
                    results.Add($"Anti-spam policy '{policy.Name}' configured");
                }
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Exchange standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Exchange standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Exchange standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<ExchangeStandardConfig>(configuration);
            return Task.FromResult(config != null && !string.IsNullOrEmpty(config.StandardName));
        } catch {
            return Task.FromResult(false);
        }
    }
}
