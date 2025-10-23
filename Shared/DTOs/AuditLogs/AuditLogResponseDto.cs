namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogResponseDto(
    List<AuditLogDto> Results,
    AuditLogMetadataDto Metadata
);