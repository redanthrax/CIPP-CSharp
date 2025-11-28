using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class AlertConfiguration : IEntityConfiguration<AlertConfiguration> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string AlertType { get; set; } = string.Empty; 
    public required string LogType { get; set; } = string.Empty;
    public required string TenantFilter { get; set; } = string.Empty; 
    public string ExcludedTenants { get; set; } = string.Empty;
    public required string Conditions { get; set; } = string.Empty;
    public required string Actions { get; set; } = string.Empty;
    public string? ScheduleCron { get; set; }
    public string? HangfireJobId { get; set; }
    public string? AlertComment { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastExecuted { get; set; }
    public string? LastExecutionResult { get; set; }
    public string? WebhookSubscriptionId { get; set; }
    public string? RawConfiguration { get; set; }

    public static string EntityName => "AlertConfigurations";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AlertConfiguration>(entity => {
            entity.ToTable("AlertConfigurations");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.AlertType);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.HangfireJobId).IsUnique().HasFilter("\"HangfireJobId\" IS NOT NULL");
            
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.AlertType).HasMaxLength(50);
            entity.Property(e => e.LogType).HasMaxLength(100);
            entity.Property(e => e.ScheduleCron).HasMaxLength(100);
            entity.Property(e => e.HangfireJobId).HasMaxLength(200);
            entity.Property(e => e.WebhookSubscriptionId).HasMaxLength(200);
            entity.Property(e => e.LastExecutionResult).HasMaxLength(1000);
            entity.Property(e => e.AlertComment).HasMaxLength(2000);
        });
    }
}