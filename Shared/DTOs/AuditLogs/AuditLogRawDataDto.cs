namespace CIPP.Shared.DTOs.AuditLogs;

public record AuditLogRawDataDto(
    DateTime CreationTime,
    string? CIPPUserKey = null,
    string? UserId = null,
    string? UserKey = null,
    string? ClientIP = null,
    string? CIPPAction = null,
    string? CIPPClause = null,
    string? Operation = null,
    string? Workload = null,
    string? ResultStatus = null,
    string? ObjectId = null,
    int? RecordType = null,
    string? OrganizationId = null,
    int? UserType = null,
    Dictionary<string, object>? AdditionalProperties = null
);