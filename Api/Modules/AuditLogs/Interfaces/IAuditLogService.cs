using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Interfaces;

public interface IAuditLogService {
    Task<AuditLogResponseDto> GetAuditLogsAsync(
        string? tenantFilter = null,
        string? logId = null,
        string? startDate = null,
        string? endDate = null,
        string? relativeTime = null,
        CancellationToken cancellationToken = default);
}