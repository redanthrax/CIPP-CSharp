using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Models;

public class TenantDomain : IEntityConfiguration<TenantDomain>
{
    public Guid Id { get; set; }
    public required Guid TenantId { get; set; }
    public required string DomainName { get; set; }
    public bool IsInitial { get; set; }
    public bool IsDefault { get; set; }
    public bool IsVerified { get; set; }
    public string? AuthenticationType { get; set; }
    public string? AvailabilityStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Tenant Tenant { get; set; } = null!;
    
    public static string EntityName => "TenantDomains";
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantDomain>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.DomainName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.AuthenticationType).HasMaxLength(50);
            entity.Property(e => e.AvailabilityStatus).HasMaxLength(50);
            
            entity.HasOne(e => e.Tenant)
                  .WithMany(t => t.Domains)
                  .HasForeignKey(e => e.TenantId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasIndex(e => e.DomainName);
            entity.HasIndex(e => new { e.TenantId, e.IsInitial }).IsUnique()
                  .HasFilter("\"IsInitial\" = true");
        });
    }
}