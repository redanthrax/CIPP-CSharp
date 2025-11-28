using CIPP.Api.Modules.Alerts.Models;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAuditLogProcessor {
    Task ProcessCachedEventsAsync(string? tenantFilter = null, int batchSize = 100);
    Task<int> ProcessEventBatchAsync(List<CachedWebhookEvent> events);
    Task EnrichAuditDataAsync(dynamic auditData, string tenantId);
    Task<List<WebhookRule>> EvaluateRulesAsync(Dictionary<string, object> enrichedData, string tenantId);
    Task ExecuteActionsAsync(WebhookRule rule, dynamic auditData, string tenantId);
}
