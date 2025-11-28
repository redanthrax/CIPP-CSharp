using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Alerts.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class ReceiveGraphWebhookCommandHandler : IRequestHandler<ReceiveGraphWebhookCommand, Task> {
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ReceiveGraphWebhookCommandHandler> _logger;

    public ReceiveGraphWebhookCommandHandler(
        ApplicationDbContext dbContext,
        ILogger<ReceiveGraphWebhookCommandHandler> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(ReceiveGraphWebhookCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Processing webhook notification payload");

            var notification = JsonSerializer.Deserialize<GraphNotificationPayload>(
                request.NotificationPayload, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (notification?.Value == null || !notification.Value.Any()) {
                _logger.LogWarning("Received empty or invalid notification payload");
                return;
            }

            foreach (var item in notification.Value) {
                try {
                    var subscription = await _dbContext.GetEntitySet<GraphWebhookSubscription>()
                        .FirstOrDefaultAsync(s => s.SubscriptionId == item.SubscriptionId && s.IsActive, 
                            cancellationToken);

                    if (subscription == null) {
                        _logger.LogWarning("Received notification for unknown subscription {SubscriptionId}", 
                            item.SubscriptionId);
                        continue;
                    }

                    if (!string.IsNullOrEmpty(subscription.ClientState) && 
                        item.ClientState != subscription.ClientState) {
                        _logger.LogWarning(
                            "Client state mismatch for subscription {SubscriptionId}",
                            item.SubscriptionId);
                        continue;
                    }

                    var cachedEvent = new CachedWebhookEvent {
                        Id = Guid.NewGuid(),
                        TenantFilter = subscription.TenantId,
                        NotificationData = JsonSerializer.Serialize(item),
                        ResourceData = item.ResourceData != null ? 
                            JsonSerializer.Serialize(item.ResourceData) : "{}",
                        ChangeType = item.ChangeType,
                        Resource = item.Resource,
                        SubscriptionId = item.SubscriptionId,
                        ReceivedAt = DateTime.UtcNow,
                        IsProcessed = false
                    };

                    _dbContext.GetEntitySet<CachedWebhookEvent>().Add(cachedEvent);
                    
                    _logger.LogInformation(
                        "Cached webhook event {EventId} for tenant {TenantId}",
                        cachedEvent.Id, subscription.TenantId);
                } catch (Exception ex) {
                    _logger.LogError(ex, "Error processing notification item");
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process webhook notification");
            throw;
        }
    }
}
