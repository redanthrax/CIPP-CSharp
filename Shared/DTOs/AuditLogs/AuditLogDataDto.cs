namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogDataDto(
    string? IP,
    AuditLogRawDataDto RawData,
    string? ActionUrl = null,
    string? ActionText = null
);