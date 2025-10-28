using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Interfaces;

public interface IBpaAnalyzer {
    string Category { get; }
    int MaxScore { get; }
    
    Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<int> CalculateScoreAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
