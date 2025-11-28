using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Models;

public class EmailTemplate : IEntityConfiguration<EmailTemplate> {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required string TemplateType { get; set; } = string.Empty;
    public required string Subject { get; set; } = string.Empty;
    public required string Body { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsSystemTemplate { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public static string EntityName => "EmailTemplates";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<EmailTemplate>(entity => {
            entity.ToTable("EmailTemplates");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Name, e.TemplateType }).IsUnique();
            entity.HasIndex(e => e.IsActive);
            
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.TemplateType).HasMaxLength(100);
            entity.Property(e => e.Subject).HasMaxLength(500);
        });
    }
}
