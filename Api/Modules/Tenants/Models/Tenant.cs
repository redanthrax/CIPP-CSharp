using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CIPP.Api.Modules.Tenants.Models;
public class Tenant : IEntityConfiguration<Tenant>
{
    // Existing core properties
    public required Guid Id { get; set; }
    public required string TenantId { get; set; }
    public required string DisplayName { get; set; }
    public required string DefaultDomainName { get; set; }
    public required string Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public string? Metadata { get; set; }
    
    public string? InitialDomainName { get; set; }
    public List<string> DomainList { get; set; } = new();
    public int GraphErrorCount { get; set; } = 0;
    public DateTime? LastSyncAt { get; set; }
    
    public TenantCapabilities? Capabilities { get; set; }
    
    public virtual ICollection<TenantProperty> Properties { get; set; } = new List<TenantProperty>();
    public virtual ICollection<TenantDomain> Domains { get; set; } = new List<TenantDomain>();

    public static string EntityName => "Tenants";

    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DefaultDomainName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Metadata).HasMaxLength(500);
            
            entity.Property(e => e.InitialDomainName).HasMaxLength(200);
            
            entity.Property(e => e.DomainList)
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                      v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>());
            
            entity.OwnsOne(e => e.Capabilities, cap =>
            {
                cap.Property(c => c.HasExchange);
                cap.Property(c => c.HasSharePoint);
                cap.Property(c => c.HasTeams);
                cap.Property(c => c.HasIntune);
                cap.Property(c => c.HasDefender);
                cap.Property(c => c.Licenses)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                       v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>());
            });
            
            entity.HasIndex(e => e.TenantId).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.LastSyncAt);
        });
    }
}
