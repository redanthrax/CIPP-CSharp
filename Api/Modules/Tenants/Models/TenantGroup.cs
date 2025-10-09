using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Models;

public class TenantGroup : IEntityConfiguration<TenantGroup> {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TenantGroupMembership> Memberships { get; set; } = new List<TenantGroupMembership>();

    public static string EntityName => "TenantGroups";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TenantGroup>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.UpdatedBy);

            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}