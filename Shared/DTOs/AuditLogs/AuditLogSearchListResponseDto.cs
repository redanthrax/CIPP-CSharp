namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogSearchListResponseDto(
    List<AuditLogSearchDto> Results,
    AuditLogSearchListMetadataDto Metadata
);