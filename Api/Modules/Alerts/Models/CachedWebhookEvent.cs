using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class CachedWebhookEvent : IEntityConfiguration<CachedWebhookEvent> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantFilter { get; set; } = string.Empty;
    public required string NotificationData { get; set; } = string.Empty;
    public required string ResourceData { get; set; } = string.Empty;
    public string? ChangeType { get; set; }
    public string? Resource { get; set; }
    public string? SubscriptionId { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public bool IsProcessed { get; set; } = false;
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; } = 0;
    public string? ErrorMessage { get; set; }

    public static string EntityName => "CachedWebhookEvents";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<CachedWebhookEvent>(entity => {
            entity.ToTable("CachedWebhookEvents");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantFilter, e.IsProcessed });
            entity.HasIndex(e => e.ReceivedAt);
            entity.HasIndex(e => e.SubscriptionId);
            
            entity.Property(e => e.ChangeType).HasMaxLength(50);
            entity.Property(e => e.Resource).HasMaxLength(500);
            entity.Property(e => e.SubscriptionId).HasMaxLength(200);
            entity.Property(e => e.ErrorMessage).HasMaxLength(2000);
        });
    }
}
