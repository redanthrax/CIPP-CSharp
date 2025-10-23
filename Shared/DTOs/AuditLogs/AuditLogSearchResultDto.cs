namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogSearchResultDto(
    List<AuditLogDto> Results,
    AuditLogSearchMetadataDto Metadata
);