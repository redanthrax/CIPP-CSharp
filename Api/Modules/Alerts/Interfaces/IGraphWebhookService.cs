using CIPP.Api.Modules.Alerts.Models;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IGraphWebhookService {
    Task<GraphWebhookSubscription> CreateSubscriptionAsync(string tenantId, string resource, string changeType, Guid? webhookRuleId = null);
    Task<GraphWebhookSubscription> RenewSubscriptionAsync(Guid subscriptionId);
    Task DeleteSubscriptionAsync(Guid subscriptionId);
    Task<List<GraphWebhookSubscription>> GetActiveSubscriptionsAsync(string? tenantId = null);
    Task<List<GraphWebhookSubscription>> GetExpiringSubscriptionsAsync(int hoursUntilExpiration = 24);
    Task RenewExpiringSubscriptionsAsync();
}
