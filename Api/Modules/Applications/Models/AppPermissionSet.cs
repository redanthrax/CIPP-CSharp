using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Applications.Models;

public class AppPermissionSet : IEntityConfiguration<AppPermissionSet> {
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PermissionsJson { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public static string EntityName => "AppPermissionSets";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AppPermissionSet>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TemplateName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.PermissionsJson).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);
            entity.Property(e => e.UpdatedOn);
            
            entity.HasIndex(e => e.TemplateName);
            entity.HasIndex(e => e.CreatedOn);
        });
    }
}
