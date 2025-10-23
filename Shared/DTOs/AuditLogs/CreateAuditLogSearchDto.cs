namespace CIPP.Shared.DTOs.AuditLogs;

public record CreateAuditLogSearchDto(
    string TenantFilter,
    DateTime StartTime,
    DateTime EndTime,
    string? DisplayName = null,
    List<string>? Operations = null,
    List<string>? UserIds = null,
    List<int>? RecordTypes = null,
    Dictionary<string, object>? AdditionalFilters = null
);