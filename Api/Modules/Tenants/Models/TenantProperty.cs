using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Models;

public class TenantProperty : IEntityConfiguration<TenantProperty>
{
    public Guid Id { get; set; }
    public required Guid TenantId { get; set; }
    public required string Key { get; set; }
    public string? Value { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Tenant Tenant { get; set; } = null!;
    
    public static string EntityName => "TenantProperties";
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantProperty>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(4000);
            
            entity.HasOne(e => e.Tenant)
                  .WithMany(t => t.Properties)
                  .HasForeignKey(e => e.TenantId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasIndex(e => new { e.TenantId, e.Key }).IsUnique();
        });
    }
}