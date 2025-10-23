namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogSearchDto(
    string SearchId,
    DateTime StartTime,
    DateTime EndTime,
    string DisplayName,
    string Tenant,
    string Status,
    Dictionary<string, object>? Query = null,
    List<string>? MatchedRules = null,
    int TotalLogs = 0,
    int MatchedLogs = 0,
    DateTime? CreatedAt = null,
    DateTime? ProcessedAt = null,
    string? ErrorMessage = null
);