using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;

public class ApplicationUser : IEntityConfiguration<ApplicationUser> {
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public string? AzureAdObjectId { get; set; }
    public required bool IsFirstUser { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? Metadata { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public static string EntityName => "ApplicationUser";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ApplicationUser>(entity => {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(250);
                
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.AzureAdObjectId)
                .HasMaxLength(50);
                
            entity.Property(e => e.IsFirstUser)
                .IsRequired();
                
            entity.Property(e => e.IsActive)
                .IsRequired();
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.Metadata)
                .HasMaxLength(1000);

            entity.HasIndex(e => e.Email)
                .IsUnique();
                
            entity.HasIndex(e => e.AzureAdObjectId)
                .IsUnique()
                .HasFilter("\"AzureAdObjectId\" IS NOT NULL");
        });
    }
}
