using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Models;

public class TenantGroupMembership : IEntityConfiguration<TenantGroupMembership> {
    public required Guid Id { get; set; }
    public required Guid TenantGroupId { get; set; }
    public required Guid TenantId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Guid CreatedBy { get; set; }

    public virtual TenantGroup TenantGroup { get; set; } = null!;
    public virtual Tenant Tenant { get; set; } = null!;

    public static string EntityName => "TenantGroupMemberships";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TenantGroupMembership>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantGroupId).IsRequired();
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired();

            entity.HasOne(e => e.TenantGroup)
                  .WithMany(g => g.Memberships)
                  .HasForeignKey(e => e.TenantGroupId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tenant)
                  .WithMany()
                  .HasForeignKey(e => e.TenantId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TenantGroupId, e.TenantId }).IsUnique();
            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}