using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Interfaces;

public interface IBpaService {
    Task<BpaReportDto> GetReportAsync(Guid tenantId, string? category = null, CancellationToken cancellationToken = default);
    Task<List<BpaRecommendationDto>> GetRecommendationsAsync(Guid tenantId, string? severity = null, string? category = null, CancellationToken cancellationToken = default);
    Task<ComplianceScoreDto> GetComplianceScoreAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
