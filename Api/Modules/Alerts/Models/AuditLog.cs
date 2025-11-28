using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class AuditLog : IEntityConfiguration<AuditLog> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string TenantFilter { get; set; } = string.Empty;
    public required string Title { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
    public required string RawData { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? LocationInfo { get; set; }
    public string? ActionsTaken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? WebhookRuleId { get; set; }
    public WebhookRule? WebhookRule { get; set; }

    public static string EntityName => "AuditLogs";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AuditLog>(entity => {
            entity.ToTable("AuditLogs");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TenantFilter);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.WebhookRuleId);
            
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            
            entity.HasOne(e => e.WebhookRule)
                .WithMany()
                .HasForeignKey(e => e.WebhookRuleId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
