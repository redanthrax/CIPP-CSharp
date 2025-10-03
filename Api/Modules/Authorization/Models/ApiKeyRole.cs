using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class ApiKeyRole : IEntityConfiguration<ApiKeyRole> {
    public required Guid ApiKeyId { get; set; }
    public required Guid RoleId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public virtual ApiKey ApiKey { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;

    public static string EntityName => "ApiKeyRoles";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ApiKeyRole>(entity => {
            entity.HasKey(e => new { e.ApiKeyId, e.RoleId });
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasOne(e => e.ApiKey)
                .WithMany(a => a.ApiKeyRoles)
                .HasForeignKey(e => e.ApiKeyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.ApiKeyRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
