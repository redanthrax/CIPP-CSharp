using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class GraphWebhookSubscription : IEntityConfiguration<GraphWebhookSubscription> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantId { get; set; } = string.Empty;
    public required string SubscriptionId { get; set; } = string.Empty;
    public required string Resource { get; set; } = string.Empty;
    public required string ChangeType { get; set; } = string.Empty;
    public required string NotificationUrl { get; set; } = string.Empty;
    public string? ClientState { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastRenewed { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? WebhookRuleId { get; set; }
    public WebhookRule? WebhookRule { get; set; }

    public static string EntityName => "GraphWebhookSubscriptions";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<GraphWebhookSubscription>(entity => {
            entity.ToTable("GraphWebhookSubscriptions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.Resource });
            entity.HasIndex(e => e.SubscriptionId).IsUnique();
            entity.HasIndex(e => e.ExpirationDateTime);
            entity.HasIndex(e => e.IsActive);
            
            entity.Property(e => e.SubscriptionId).HasMaxLength(200);
            entity.Property(e => e.Resource).HasMaxLength(500);
            entity.Property(e => e.ChangeType).HasMaxLength(100);
            entity.Property(e => e.NotificationUrl).HasMaxLength(500);
            entity.Property(e => e.ClientState).HasMaxLength(200);
            
            entity.HasOne(e => e.WebhookRule)
                .WithMany()
                .HasForeignKey(e => e.WebhookRuleId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
