using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CIPP.Api.Modules.AuditLogs.Models;

public class AuditLogSearch : IEntityConfiguration<AuditLogSearch> {
    public required Guid Id { get; set; }
    public required string SearchId { get; set; }
    public required string Tenant { get; set; }
    public required string DisplayName { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int TotalLogs { get; set; }
    public int MatchedLogs { get; set; }
    public string? ErrorMessage { get; set; }
    
    public string? QueryJson { get; set; }
    public string? MatchedRulesJson { get; set; }

    [NotMapped]
    public Dictionary<string, object>? Query {
        get => string.IsNullOrEmpty(QueryJson) 
            ? null 
            : JsonSerializer.Deserialize<Dictionary<string, object>>(QueryJson);
        set => QueryJson = value == null 
            ? null 
            : JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public List<string>? MatchedRules {
        get => string.IsNullOrEmpty(MatchedRulesJson) 
            ? null 
            : JsonSerializer.Deserialize<List<string>>(MatchedRulesJson);
        set => MatchedRulesJson = value == null 
            ? null 
            : JsonSerializer.Serialize(value);
    }

    public static string EntityName => "AuditLogSearches";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AuditLogSearch>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SearchId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tenant).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.ProcessedAt);
            entity.Property(e => e.TotalLogs).HasDefaultValue(0);
            entity.Property(e => e.MatchedLogs).HasDefaultValue(0);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            entity.Property(e => e.QueryJson).HasColumnType("jsonb");
            entity.Property(e => e.MatchedRulesJson).HasColumnType("jsonb");

            entity.HasIndex(e => e.SearchId);
            entity.HasIndex(e => e.Tenant);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.StartTime);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}