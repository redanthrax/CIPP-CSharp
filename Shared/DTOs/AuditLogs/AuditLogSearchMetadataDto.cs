namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogSearchMetadataDto(
    string SearchId,
    string TenantFilter,
    int TotalResults,
    string Status,
    DateTime StartTime,
    DateTime EndTime,
    string? ErrorMessage = null
);