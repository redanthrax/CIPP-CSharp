using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Interfaces;

public interface IAuditLogSearchService {
    Task<AuditLogSearchListResponseDto> GetSearchesAsync(
        string tenantFilter,
        int? days = null,
        CancellationToken cancellationToken = default);

    Task<AuditLogSearchResultDto> GetSearchResultsAsync(
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default);

    Task<AuditLogSearchDto> CreateSearchAsync(
        CreateAuditLogSearchDto request,
        CancellationToken cancellationToken = default);

    Task<AuditLogSearchDto> ProcessSearchAsync(
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default);
}