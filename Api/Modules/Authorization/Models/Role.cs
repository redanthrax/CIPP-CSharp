using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class Role : IEntityConfiguration<Role> {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsBuiltIn { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<ApiKeyRole> ApiKeyRoles { get; set; } = new List<ApiKeyRole>();

    public static string EntityName => "Roles";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Role>(entity => {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
                
            entity.Property(e => e.IsBuiltIn)
                .IsRequired();
                
            entity.Property(e => e.IsActive)
                .IsRequired();
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
    }
}
