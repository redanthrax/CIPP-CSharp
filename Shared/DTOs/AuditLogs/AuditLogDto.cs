namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogDto(
    string LogId,
    DateTime Timestamp,
    string Tenant,
    string Title,
    AuditLogDataDto Data
);