using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class WebhookRule : IEntityConfiguration<WebhookRule> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantFilter { get; set; } = string.Empty;
    public string ExcludedTenants { get; set; } = string.Empty;
    public required string Conditions { get; set; } = string.Empty;
    public required string Actions { get; set; } = string.Empty;
    public required string LogType { get; set; } = string.Empty;
    public string? AlertComment { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastTriggered { get; set; }
    public int TriggerCount { get; set; } = 0;

    public static string EntityName => "WebhookRules";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<WebhookRule>(entity => {
            entity.ToTable("WebhookRules");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TenantFilter);
            entity.HasIndex(e => e.LogType);
            entity.HasIndex(e => e.IsActive);
            
            entity.Property(e => e.LogType).HasMaxLength(100);
            entity.Property(e => e.AlertComment).HasMaxLength(2000);
        });
    }
}
