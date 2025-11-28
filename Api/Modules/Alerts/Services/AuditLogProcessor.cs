using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Shared.DTOs.Alerts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AuditLogProcessor : IAuditLogProcessor {
    private readonly ApplicationDbContext _dbContext;
    private readonly AuditDataEnricher _dataEnricher;
    private readonly AlertRuleEvaluator _ruleEvaluator;
    private readonly IEnumerable<IAlertActionHandler> _actionHandlers;
    private readonly ILogger<AuditLogProcessor> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditLogProcessor(
        ApplicationDbContext dbContext,
        AuditDataEnricher dataEnricher,
        AlertRuleEvaluator ruleEvaluator,
        IEnumerable<IAlertActionHandler> actionHandlers,
        ILogger<AuditLogProcessor> logger) {
        _dbContext = dbContext;
        _dataEnricher = dataEnricher;
        _ruleEvaluator = ruleEvaluator;
        _actionHandlers = actionHandlers;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task ProcessCachedEventsAsync(string? tenantFilter = null, int batchSize = 100) {
        var query = _dbContext.GetEntitySet<CachedWebhookEvent>()
            .Where(e => !e.IsProcessed && e.RetryCount < 3)
            .OrderBy(e => e.ReceivedAt)
            .Take(batchSize);

        if (!string.IsNullOrEmpty(tenantFilter)) {
            query = (IOrderedQueryable<CachedWebhookEvent>)query.Where(e => e.TenantFilter == tenantFilter);
        }

        var events = await query.ToListAsync();

        if (!events.Any()) {
            return;
        }

        _logger.LogInformation("Processing {Count} cached webhook events", events.Count);
        await ProcessEventBatchAsync(events);
    }

    public async Task<int> ProcessEventBatchAsync(List<CachedWebhookEvent> events) {
        var processedCount = 0;

        foreach (var cachedEvent in events) {
            try {
                var resourceData = JsonDocument.Parse(cachedEvent.ResourceData).RootElement;
                
                var enrichedData = await _dataEnricher.EnrichAuditDataAsync(
                    resourceData, 
                    cachedEvent.TenantFilter);

                var matchedRules = await EvaluateRulesAsync(enrichedData, cachedEvent.TenantFilter);

                foreach (var rule in matchedRules) {
                    try {
                        await ExecuteActionsAsync(rule, enrichedData, cachedEvent.TenantFilter);
                        
                        await SaveAuditLogAsync(cachedEvent, rule, enrichedData);
                        
                        rule.LastTriggered = DateTime.UtcNow;
                        rule.TriggerCount++;
                    } catch (Exception ex) {
                        _logger.LogError(ex, 
                            "Error executing actions for rule {RuleId} on event {EventId}",
                            rule.Id, cachedEvent.Id);
                    }
                }

                cachedEvent.IsProcessed = true;
                cachedEvent.ProcessedAt = DateTime.UtcNow;
                processedCount++;
            } catch (Exception ex) {
                _logger.LogError(ex, "Error processing event {EventId}", cachedEvent.Id);
                cachedEvent.RetryCount++;
                cachedEvent.ErrorMessage = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successfully processed {Count} events", processedCount);
        
        return processedCount;
    }

    public Task EnrichAuditDataAsync(dynamic auditData, string tenantId) {
        throw new NotImplementedException("Use AuditDataEnricher directly");
    }

    public async Task<List<WebhookRule>> EvaluateRulesAsync(
        Dictionary<string, object> enrichedData, 
        string tenantId) {
        var matchedRules = new List<WebhookRule>();

        try {
            var rules = await _dbContext.GetEntitySet<WebhookRule>()
                .Where(r => r.IsActive)
                .Where(r => r.TenantFilter == tenantId || r.TenantFilter == "AllTenants")
                .ToListAsync();

            foreach (var rule in rules) {
                try {
                    var excludedTenants = JsonSerializer.Deserialize<List<string>>(
                        rule.ExcludedTenants, 
                        _jsonOptions) ?? new List<string>();
                    
                    if (excludedTenants.Contains(tenantId)) {
                        continue;
                    }

                    var conditions = JsonSerializer.Deserialize<List<AlertConditionDto>>(
                        rule.Conditions, 
                        _jsonOptions) ?? new List<AlertConditionDto>();

                    if (_ruleEvaluator.EvaluateConditions(conditions, enrichedData)) {
                        matchedRules.Add(rule);
                        _logger.LogInformation(
                            "Rule {RuleId} matched for tenant {TenantId}",
                            rule.Id, tenantId);
                    }
                } catch (Exception ex) {
                    _logger.LogError(ex, "Error evaluating rule {RuleId}", rule.Id);
                }
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Error loading rules for tenant {TenantId}", tenantId);
        }

        return matchedRules;
    }

    public async Task ExecuteActionsAsync(WebhookRule rule, dynamic auditData, string tenantId) {
        try {
            var actions = JsonSerializer.Deserialize<List<AlertActionDto>>(
                rule.Actions, 
                _jsonOptions) ?? new List<AlertActionDto>();

            if (!actions.Any()) {
                _logger.LogInformation("No actions configured for rule {RuleId}", rule.Id);
                return;
            }

            var enrichedData = auditData as Dictionary<string, object> ?? new Dictionary<string, object>();

            foreach (var action in actions) {
                try {
                    var handler = _actionHandlers.FirstOrDefault(h => 
                        h.ActionType.Equals(action.ActionType, StringComparison.OrdinalIgnoreCase));

                    if (handler == null) {
                        _logger.LogWarning(
                            "No handler found for action type {ActionType} in rule {RuleId}",
                            action.ActionType, rule.Id);
                        continue;
                    }

                    var additionalParams = new Dictionary<string, string>();
                    if (action.Parameters != null) {
                        foreach (var param in action.Parameters) {
                            additionalParams[param.Key] = param.Value?.ToString() ?? string.Empty;
                        }
                    }

                    await handler.ExecuteAsync(
                        enrichedData,
                        tenantId,
                        rule.AlertComment,
                        additionalParams);

                    _logger.LogInformation(
                        "Successfully executed action {ActionType} for rule {RuleId}",
                        action.ActionType, rule.Id);
                } catch (Exception ex) {
                    _logger.LogError(ex, 
                        "Failed to execute action {ActionType} for rule {RuleId}",
                        action.ActionType, rule.Id);
                }
            }
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Error executing actions for rule {RuleId} on tenant {TenantId}",
                rule.Id, tenantId);
        }
    }

    private async Task SaveAuditLogAsync(
        CachedWebhookEvent cachedEvent, 
        WebhookRule rule, 
        Dictionary<string, object> enrichedData) {
        try {
            var auditLog = new AuditLog {
                Id = Guid.NewGuid(),
                TenantFilter = cachedEvent.TenantFilter,
                Title = $"Alert: {rule.LogType}",
                RawData = JsonSerializer.Serialize(enrichedData),
                IpAddress = enrichedData.TryGetValue("ClientIP", out var ip) ? ip.ToString() : null,
                WebhookRuleId = rule.Id,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.GetEntitySet<AuditLog>().Add(auditLog);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation(
                "Saved audit log {AuditLogId} for event {EventId}",
                auditLog.Id, cachedEvent.Id);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to save audit log for event {EventId}", cachedEvent.Id);
        }
    }
}
