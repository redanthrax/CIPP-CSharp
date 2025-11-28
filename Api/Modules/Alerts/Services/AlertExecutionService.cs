using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Shared.DTOs.Alerts;
using CIPP.Shared.DTOs.Tenants;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertExecutionService {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMicrosoftGraphService _graphService;
    private readonly AlertRuleEvaluator _ruleEvaluator;
    private readonly IEnumerable<IAlertActionHandler> _actionHandlers;
    private readonly ILogger<AlertExecutionService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AlertExecutionService(
        ApplicationDbContext dbContext,
        IMicrosoftGraphService graphService,
        AlertRuleEvaluator ruleEvaluator,
        IEnumerable<IAlertActionHandler> actionHandlers,
        ILogger<AlertExecutionService> logger) {
        _dbContext = dbContext;
        _graphService = graphService;
        _ruleEvaluator = ruleEvaluator;
        _actionHandlers = actionHandlers;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task ExecuteAlertAsync(Guid alertConfigurationId) {
        AlertConfiguration? alertConfig = null;
        try {
            alertConfig = await _dbContext.GetEntitySet<AlertConfiguration>()
                .FirstOrDefaultAsync(a => a.Id == alertConfigurationId && a.IsActive);

            if (alertConfig == null) {
                _logger.LogWarning("Alert configuration {ConfigurationId} not found or is inactive", alertConfigurationId);
                return;
            }

            _logger.LogInformation("Executing alert {ConfigurationId} of type {AlertType}", 
                alertConfigurationId, alertConfig.AlertType);

            alertConfig.LastExecuted = DateTime.UtcNow;

            string result;
            switch (alertConfig.AlertType.ToLowerInvariant()) {
                case "auditlog":
                    result = await ExecuteAuditLogAlertAsync(alertConfig);
                    break;
                case "scripted":
                    result = await ExecuteScriptedAlertAsync(alertConfig);
                    break;
                default:
                    result = $"Unknown alert type: {alertConfig.AlertType}";
                    _logger.LogWarning("Unknown alert type {AlertType} for configuration {ConfigurationId}", 
                        alertConfig.AlertType, alertConfigurationId);
                    break;
            }

            alertConfig.LastExecutionResult = result.Length > 1000 ? result[..1000] : result;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully executed alert {ConfigurationId}: {Result}", 
                alertConfigurationId, result);

        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to execute alert {ConfigurationId}", alertConfigurationId);
            
            if (alertConfig != null) {
                try {
                    alertConfig.LastExecutionResult = $"Error: {ex.Message}";
                    await _dbContext.SaveChangesAsync();
                } catch (Exception saveEx) {
                    _logger.LogError(saveEx, "Failed to save execution error for alert {ConfigurationId}", alertConfigurationId);
                }
            }
            throw;
        }
    }

    private async Task<string> ExecuteAuditLogAlertAsync(AlertConfiguration alertConfig) {
        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync();
            
            if (!string.IsNullOrEmpty(alertConfig.WebhookSubscriptionId)) {
                var subscription = await graphClient.Subscriptions[alertConfig.WebhookSubscriptionId].GetAsync();
                
                if (subscription?.ExpirationDateTime < DateTime.UtcNow.AddHours(1)) {
                    subscription.ExpirationDateTime = DateTime.UtcNow.AddDays(3);
                    await graphClient.Subscriptions[alertConfig.WebhookSubscriptionId].PatchAsync(subscription);
                    return "Renewed expiring webhook subscription";
                }
                
                return "Webhook subscription is active and healthy";
            }
            
            return "No webhook subscription to check";
            
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to execute audit log alert {ConfigurationId}", alertConfig.Id);
            return $"Failed to execute audit log alert: {ex.Message}";
        }
    }

    private async Task<string> ExecuteScriptedAlertAsync(AlertConfiguration alertConfig) {
        try {
            var rawConfig = JsonSerializer.Deserialize<CreateScriptedAlertDto>(
                alertConfig.RawConfiguration ?? "{}", 
                _jsonOptions);

            if (rawConfig?.Command == null) {
                return "No command configured for scripted alert";
            }

            var tenantIds = GetTenantIds(alertConfig.TenantFilter);
            var excludedTenants = JsonSerializer.Deserialize<List<string>>(
                alertConfig.ExcludedTenants, 
                _jsonOptions) ?? new List<string>();

            var tenantsToCheck = tenantIds.Except(excludedTenants).ToList();
            var triggeredCount = 0;
            var totalChecked = 0;

            foreach (var tenantId in tenantsToCheck) {
                try {
                    totalChecked++;
                    var data = await FetchCommandDataAsync(rawConfig.Command.Value, tenantId, rawConfig.Parameters);
                    
                    if (data != null && data.Any()) {
                        var alertData = new Dictionary<string, object> {
                            ["Command"] = rawConfig.Command.Value,
                            ["TenantId"] = tenantId,
                            ["DataCount"] = data.Count,
                            ["Data"] = data,
                            ["Timestamp"] = DateTime.UtcNow
                        };

                        await ExecuteAlertActionsAsync(
                            alertData,
                            tenantId,
                            alertConfig.AlertComment,
                            rawConfig.PostExecution);

                        triggeredCount++;
                    }
                } catch (Exception ex) {
                    _logger.LogError(ex, 
                        "Failed to execute scripted alert for tenant {TenantId}",
                        tenantId);
                }
            }

            return $"Checked {totalChecked} tenants, triggered {triggeredCount} alerts";
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to execute scripted alert {ConfigurationId}", alertConfig.Id);
            return $"Failed to execute scripted alert: {ex.Message}";
        }
    }

    private List<string> GetTenantIds(string tenantFilterJson) {
        try {
            var tenants = JsonSerializer.Deserialize<List<TenantSelectorOptionDto>>(
                tenantFilterJson, 
                _jsonOptions) ?? new List<TenantSelectorOptionDto>();

            if (tenants.Any(t => t.Value == "AllTenants")) {
                var allTenants = _dbContext.GetEntitySet<Tenant>()
                    .Where(t => !t.Excluded && t.Status == "Active")
                    .Select(t => t.TenantId.ToString())
                    .ToList();
                return allTenants;
            }

            return tenants.Select(t => t.Value).ToList();
        } catch {
            return new List<string>();
        }
    }

    private async Task<List<Dictionary<string, object>>> FetchCommandDataAsync(
        string command,
        string tenantId,
        Dictionary<string, object>? parameters) {
        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));

            return command switch {
                "New-GraphGetRequest" => await FetchGraphDataAsync(graphClient, parameters),
                "Get-CIPPMFAUsers" => await FetchMfaUsersAsync(graphClient),
                "Get-CIPPLicenses" => await FetchLicensesAsync(graphClient),
                _ => new List<Dictionary<string, object>>()
            };
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Failed to fetch data for command {Command} on tenant {TenantId}",
                command, tenantId);
            return new List<Dictionary<string, object>>();
        }
    }

    private async Task<List<Dictionary<string, object>>> FetchGraphDataAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient,
        Dictionary<string, object>? parameters) {
        if (parameters == null || !parameters.ContainsKey("Endpoint")) {
            return new List<Dictionary<string, object>>();
        }

        var endpoint = parameters["Endpoint"].ToString();
        var results = new List<Dictionary<string, object>>();

        return results;
    }

    private async Task<List<Dictionary<string, object>>> FetchMfaUsersAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient) {
        try {
            var users = await graphClient.Users.GetAsync(config => {
                config.QueryParameters.Select = new[] { "id", "userPrincipalName", "displayName" };
                config.QueryParameters.Top = 999;
            });

            var results = new List<Dictionary<string, object>>();
            if (users?.Value != null) {
                foreach (var user in users.Value) {
                    var authMethods = await graphClient.Users[user.Id].Authentication.Methods.GetAsync();
                    
                    results.Add(new Dictionary<string, object> {
                        ["UserId"] = user.Id ?? "",
                        ["UserPrincipalName"] = user.UserPrincipalName ?? "",
                        ["DisplayName"] = user.DisplayName ?? "",
                        ["MfaMethodCount"] = authMethods?.Value?.Count ?? 0
                    });
                }
            }

            return results;
        } catch {
            return new List<Dictionary<string, object>>();
        }
    }

    private async Task<List<Dictionary<string, object>>> FetchLicensesAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient) {
        try {
            var licenses = await graphClient.SubscribedSkus.GetAsync();
            var results = new List<Dictionary<string, object>>();

            if (licenses?.Value != null) {
                foreach (var sku in licenses.Value) {
                    results.Add(new Dictionary<string, object> {
                        ["SkuId"] = sku.SkuId?.ToString() ?? "",
                        ["SkuPartNumber"] = sku.SkuPartNumber ?? "",
                        ["ConsumedUnits"] = sku.ConsumedUnits ?? 0,
                        ["PrepaidUnits"] = sku.PrepaidUnits?.Enabled ?? 0
                    });
                }
            }

            return results;
        } catch {
            return new List<Dictionary<string, object>>();
        }
    }

    private async Task ExecuteAlertActionsAsync(
        Dictionary<string, object> alertData,
        string tenantId,
        string? alertComment,
        List<PostExecutionOptionDto>? postExecutionOptions) {
        if (postExecutionOptions == null || !postExecutionOptions.Any()) {
            return;
        }

        foreach (var option in postExecutionOptions) {
            try {
                var handler = _actionHandlers.FirstOrDefault(h => 
                    h.ActionType.Equals(option.Value, StringComparison.OrdinalIgnoreCase));

                if (handler != null) {
                    await handler.ExecuteAsync(
                        alertData,
                        tenantId,
                        alertComment);
                } else {
                    _logger.LogWarning(
                        "No handler found for post-execution action {ActionType}",
                        option.Value);
                }
            } catch (Exception ex) {
                _logger.LogError(ex, 
                    "Failed to execute post-execution action {ActionType}",
                    option.Value);
            }
        }
    }
}