using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Alerts.Services;

public class GraphWebhookService : IGraphWebhookService {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMicrosoftGraphService _graphService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GraphWebhookService> _logger;

    public GraphWebhookService(
        ApplicationDbContext dbContext,
        IMicrosoftGraphService graphService,
        IConfiguration configuration,
        ILogger<GraphWebhookService> logger) {
        _dbContext = dbContext;
        _graphService = graphService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<GraphWebhookSubscription> CreateSubscriptionAsync(
        string tenantId, 
        string resource, 
        string changeType, 
        Guid? webhookRuleId = null) {
        try {
            var notificationUrl = _configuration["Webhooks:NotificationUrl"] ?? 
                throw new InvalidOperationException("Webhook notification URL not configured");

            var clientState = Guid.NewGuid().ToString();
            var expirationDateTime = DateTime.UtcNow.AddHours(48);

            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));
            
            var subscription = new Subscription {
                Resource = resource,
                ChangeType = changeType,
                NotificationUrl = notificationUrl,
                ExpirationDateTime = expirationDateTime,
                ClientState = clientState
            };

            var createdSubscription = await graphClient.Subscriptions.PostAsync(subscription);
            
            if (createdSubscription?.Id == null) {
                throw new InvalidOperationException("Failed to create Graph subscription");
            }

            var webhookSubscription = new GraphWebhookSubscription {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                SubscriptionId = createdSubscription.Id,
                Resource = resource,
                ChangeType = changeType,
                NotificationUrl = notificationUrl,
                ClientState = clientState,
                ExpirationDateTime = expirationDateTime,
                WebhookRuleId = webhookRuleId,
                IsActive = true
            };

            _dbContext.GetEntitySet<GraphWebhookSubscription>().Add(webhookSubscription);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Created Graph webhook subscription {SubscriptionId} for tenant {TenantId} on resource {Resource}",
                createdSubscription.Id, tenantId, resource);

            return webhookSubscription;
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Failed to create webhook subscription for tenant {TenantId} on resource {Resource}",
                tenantId, resource);
            throw;
        }
    }

    public async Task<GraphWebhookSubscription> RenewSubscriptionAsync(Guid subscriptionId) {
        try {
            var subscription = await _dbContext.GetEntitySet<GraphWebhookSubscription>()
                .FirstOrDefaultAsync(s => s.Id == subscriptionId && s.IsActive);

            if (subscription == null) {
                throw new InvalidOperationException($"Subscription {subscriptionId} not found");
            }

            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(subscription.TenantId));
            var newExpirationDateTime = DateTime.UtcNow.AddHours(48);

            var updatedSubscription = new Subscription {
                ExpirationDateTime = newExpirationDateTime
            };

            await graphClient.Subscriptions[subscription.SubscriptionId]
                .PatchAsync(updatedSubscription);

            subscription.ExpirationDateTime = newExpirationDateTime;
            subscription.LastRenewed = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Renewed Graph webhook subscription {SubscriptionId} for tenant {TenantId}",
                subscription.SubscriptionId, subscription.TenantId);

            return subscription;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to renew webhook subscription {SubscriptionId}", subscriptionId);
            throw;
        }
    }

    public async Task DeleteSubscriptionAsync(Guid subscriptionId) {
        try {
            var subscription = await _dbContext.GetEntitySet<GraphWebhookSubscription>()
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null) {
                _logger.LogWarning("Subscription {SubscriptionId} not found for deletion", subscriptionId);
                return;
            }

            try {
                var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(subscription.TenantId));
                await graphClient.Subscriptions[subscription.SubscriptionId].DeleteAsync();
            } catch (Exception ex) {
                _logger.LogWarning(ex, 
                    "Failed to delete subscription {SubscriptionId} from Graph, continuing with local cleanup",
                    subscription.SubscriptionId);
            }

            subscription.IsActive = false;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Deleted Graph webhook subscription {SubscriptionId} for tenant {TenantId}",
                subscription.SubscriptionId, subscription.TenantId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete webhook subscription {SubscriptionId}", subscriptionId);
            throw;
        }
    }

    public async Task<List<GraphWebhookSubscription>> GetActiveSubscriptionsAsync(string? tenantId = null) {
        var query = _dbContext.GetEntitySet<GraphWebhookSubscription>()
            .Where(s => s.IsActive);

        if (!string.IsNullOrEmpty(tenantId)) {
            query = query.Where(s => s.TenantId == tenantId);
        }

        return await query.ToListAsync();
    }

    public async Task<List<GraphWebhookSubscription>> GetExpiringSubscriptionsAsync(int hoursUntilExpiration = 24) {
        var expirationThreshold = DateTime.UtcNow.AddHours(hoursUntilExpiration);
        
        return await _dbContext.GetEntitySet<GraphWebhookSubscription>()
            .Where(s => s.IsActive && s.ExpirationDateTime <= expirationThreshold)
            .ToListAsync();
    }

    public async Task RenewExpiringSubscriptionsAsync() {
        var expiringSubscriptions = await GetExpiringSubscriptionsAsync();
        
        _logger.LogInformation("Found {Count} expiring subscriptions to renew", expiringSubscriptions.Count);

        foreach (var subscription in expiringSubscriptions) {
            try {
                await RenewSubscriptionAsync(subscription.Id);
            } catch (Exception ex) {
                _logger.LogError(ex, 
                    "Failed to renew expiring subscription {SubscriptionId} for tenant {TenantId}",
                    subscription.SubscriptionId, subscription.TenantId);
            }
        }
    }
}
