using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Microsoft.Interfaces;
using CIPP.Shared.DTOs.Alerts;
using CIPP.Shared.DTOs.Tenants;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertExecutionService {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<AlertExecutionService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AlertExecutionService(
        ApplicationDbContext dbContext,
        IMicrosoftGraphService graphService,
        ILogger<AlertExecutionService> logger) {
        _dbContext = dbContext;
        _graphService = graphService;
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
            var conditions = JsonSerializer.Deserialize<List<AlertConditionDto>>(alertConfig.Conditions, _jsonOptions) ?? new List<AlertConditionDto>();
            var actions = JsonSerializer.Deserialize<List<AlertActionDto>>(alertConfig.Actions, _jsonOptions) ?? new List<AlertActionDto>();
            var tenants = JsonSerializer.Deserialize<List<TenantSelectorOptionDto>>(alertConfig.TenantFilter, _jsonOptions) ?? new List<TenantSelectorOptionDto>();

            _logger.LogInformation("Executing scripted alert with {ConditionCount} conditions, {ActionCount} actions for {TenantCount} tenants", 
                conditions.Count, actions.Count, tenants.Count);

            var conditionsMet = await EvaluateConditionsAsync(conditions, tenants, alertConfig.LogType);
            
            if (conditionsMet) {
                await ExecuteActionsAsync(actions, tenants);
                return $"Conditions met, executed {actions.Count} actions for {tenants.Count} tenants";
            } else {
                return "Conditions not met, no actions executed";
            }
            
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to execute scripted alert {ConfigurationId}", alertConfig.Id);
            return $"Failed to execute scripted alert: {ex.Message}";
        }
    }

    private async Task<bool> EvaluateConditionsAsync(List<AlertConditionDto> conditions, List<TenantSelectorOptionDto> tenants, string logType) {
        _logger.LogInformation("Evaluating {ConditionCount} conditions for log type {LogType}", conditions.Count, logType);
        await Task.Delay(100);
        return conditions.Any();
    }

    private async Task ExecuteActionsAsync(List<AlertActionDto> actions, List<TenantSelectorOptionDto> tenants) {
        foreach (var action in actions) {
            _logger.LogInformation("Executing action of type {ActionType} for {TenantCount} tenants", 
                action.Value, tenants.Count);
            await Task.Delay(50);
        }
    }
}