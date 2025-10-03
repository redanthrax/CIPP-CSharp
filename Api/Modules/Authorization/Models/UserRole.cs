using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class UserRole : IEntityConfiguration<UserRole> {
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;

    public static string EntityName => "UserRoles";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<UserRole>(entity => {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
