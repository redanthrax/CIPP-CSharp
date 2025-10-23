using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.ConditionalAccess.Models;

public class ConditionalAccessTemplate : IEntityConfiguration<ConditionalAccessTemplate> {
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TemplateJson { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public static string EntityName => "ConditionalAccessTemplates";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ConditionalAccessTemplate>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TemplateName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.TemplateJson).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedOn);
            
            entity.HasIndex(e => e.TemplateName);
            entity.HasIndex(e => e.CreatedOn);
        });
    }
}
