using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Models;
public class ApiKey : IEntityConfiguration<ApiKey> {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastUsedAt { get; set; }
    public int UsageCount { get; set; } = 0;

    public virtual ICollection<ApiKeyRole> ApiKeyRoles { get; set; } = new List<ApiKeyRole>();

    public static string EntityName => "ApiKeys";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ApiKey>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.KeyHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ExpiresAt);
            entity.Property(e => e.IsActive).IsRequired();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.KeyHash);
        });
    }
}
