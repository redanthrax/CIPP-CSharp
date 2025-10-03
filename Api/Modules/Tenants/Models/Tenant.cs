using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Models;
public class Tenant : IEntityConfiguration<Tenant>
{
    public required Guid Id { get; set; }
    public required string TenantId { get; set; }
    public required string DisplayName { get; set; }
    public required string DefaultDomainName { get; set; }
    public required string Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public string? Metadata { get; set; }

    public static string EntityName => "Tenants";

    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DefaultDomainName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Metadata).HasMaxLength(500);
        });
    }
}
