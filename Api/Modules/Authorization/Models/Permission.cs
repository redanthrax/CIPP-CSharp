using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class Permission : IEntityConfiguration<Permission> {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required bool IsActive { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public static string EntityName => "Permissions";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Permission>(entity => {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
                
            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
    }
}
