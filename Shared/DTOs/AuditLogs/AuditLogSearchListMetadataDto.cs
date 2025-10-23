namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogSearchListMetadataDto(
    string TenantFilter,
    int TotalSearches,
    DateTime? StartTime = null
);