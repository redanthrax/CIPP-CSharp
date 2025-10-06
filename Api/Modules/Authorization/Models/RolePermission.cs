using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class RolePermission : IEntityConfiguration<RolePermission> {
    public required Guid RoleId { get; set; }
    public required Guid PermissionId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;

    public static string EntityName => "RolePermissions";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<RolePermission>(entity => {
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
