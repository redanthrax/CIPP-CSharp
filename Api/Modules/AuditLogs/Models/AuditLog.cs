using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CIPP.Api.Modules.AuditLogs.Models;

public class AuditLog : IEntityConfiguration<AuditLog> {
    public required Guid Id { get; set; }
    public required string LogId { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string Tenant { get; set; }
    public required string Title { get; set; }
    public string? IP { get; set; }
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
    
    public required DateTime CreationTime { get; set; }
    public string? CIPPUserKey { get; set; }
    public string? UserId { get; set; }
    public string? UserKey { get; set; }
    public string? ClientIP { get; set; }
    public string? CIPPAction { get; set; }
    public string? CIPPClause { get; set; }
    public string? Operation { get; set; }
    public string? Workload { get; set; }
    public string? ResultStatus { get; set; }
    public string? ObjectId { get; set; }
    public int? RecordType { get; set; }
    public string? OrganizationId { get; set; }
    public int? UserType { get; set; }
    
    public string? AdditionalPropertiesJson { get; set; }

    [NotMapped]
    public Dictionary<string, object>? AdditionalProperties {
        get => string.IsNullOrEmpty(AdditionalPropertiesJson) 
            ? null 
            : JsonSerializer.Deserialize<Dictionary<string, object>>(AdditionalPropertiesJson);
        set => AdditionalPropertiesJson = value == null 
            ? null 
            : JsonSerializer.Serialize(value);
    }

    public static string EntityName => "AuditLogs";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AuditLog>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LogId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Tenant).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.IP).HasMaxLength(45);
            entity.Property(e => e.ActionUrl).HasMaxLength(1000);
            entity.Property(e => e.ActionText).HasMaxLength(200);
            
            entity.Property(e => e.CreationTime).IsRequired();
            entity.Property(e => e.CIPPUserKey).HasMaxLength(200);
            entity.Property(e => e.UserId).HasMaxLength(200);
            entity.Property(e => e.UserKey).HasMaxLength(200);
            entity.Property(e => e.ClientIP).HasMaxLength(45);
            entity.Property(e => e.CIPPAction).HasMaxLength(500);
            entity.Property(e => e.CIPPClause).HasMaxLength(500);
            entity.Property(e => e.Operation).HasMaxLength(200);
            entity.Property(e => e.Workload).HasMaxLength(100);
            entity.Property(e => e.ResultStatus).HasMaxLength(100);
            entity.Property(e => e.ObjectId).HasMaxLength(200);
            entity.Property(e => e.RecordType);
            entity.Property(e => e.OrganizationId).HasMaxLength(100);
            entity.Property(e => e.UserType);
            entity.Property(e => e.AdditionalPropertiesJson).HasColumnType("jsonb");

            entity.HasIndex(e => e.LogId);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.Tenant);
            entity.HasIndex(e => e.CreationTime);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Operation);
            entity.HasIndex(e => e.Workload);
        });
    }
}