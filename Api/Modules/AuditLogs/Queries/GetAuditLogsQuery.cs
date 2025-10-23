using CIPP.Shared.DTOs.AuditLogs;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.AuditLogs.Queries;

public record GetAuditLogsQuery(
    string? TenantFilter = null,
    string? LogId = null,
    string? StartDate = null,
    string? EndDate = null,
    string? RelativeTime = null
) : IRequest<GetAuditLogsQuery, Task<AuditLogResponseDto>>;
