using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Tenants.Interfaces;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Services;

public class BpaService : IBpaService {
    private readonly IEnumerable<IBpaAnalyzer> _analyzers;
    private readonly ITenantCacheService _tenantService;
    private readonly ILogger<BpaService> _logger;

    public BpaService(IEnumerable<IBpaAnalyzer> analyzers, ITenantCacheService tenantService, ILogger<BpaService> logger) {
        _analyzers = analyzers;
        _tenantService = tenantService;
        _logger = logger;
    }

    public async Task<BpaReportDto> GetReportAsync(Guid tenantId, string? category = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Generating BPA report for tenant {TenantId}, category: {Category}", tenantId, category ?? "All");

        var tenant = await _tenantService.GetTenantAsync(tenantId);
        var tenantName = tenant?.DefaultDomainName ?? "Unknown";

        var analyzersToRun = string.IsNullOrEmpty(category)
            ? _analyzers
            : _analyzers.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        var categoryScores = new List<BpaCategoryScoreDto>();
        var allRecommendations = new List<BpaRecommendationDto>();

        foreach (var analyzer in analyzersToRun) {
            try {
                var recommendations = await analyzer.AnalyzeAsync(tenantId, cancellationToken);
                var score = await analyzer.CalculateScoreAsync(tenantId, cancellationToken);

                allRecommendations.AddRange(recommendations);

                categoryScores.Add(new BpaCategoryScoreDto {
                    Category = analyzer.Category,
                    Score = score,
                    MaxScore = analyzer.MaxScore,
                    ChecksPassed = recommendations.Count(r => r.Status == "Passed"),
                    ChecksFailed = recommendations.Count(r => r.Status == "Failed"),
                    ChecksWarning = recommendations.Count(r => r.Status == "Warning")
                });
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to run analyzer for category {Category}", analyzer.Category);
            }
        }

        var totalScore = categoryScores.Sum(c => c.Score);
        var maxTotalScore = categoryScores.Sum(c => c.MaxScore);
        var overallScore = maxTotalScore > 0 ? (int)((double)totalScore / maxTotalScore * 100) : 0;

        return new BpaReportDto {
            TenantId = tenantId,
            TenantName = tenantName,
            OverallScore = overallScore,
            GeneratedDate = DateTime.UtcNow,
            CategoryScores = categoryScores,
            Recommendations = allRecommendations.OrderByDescending(r => GetSeverityWeight(r.Severity)).ToList(),
            TotalChecks = allRecommendations.Count,
            PassedChecks = allRecommendations.Count(r => r.Status == "Passed"),
            FailedChecks = allRecommendations.Count(r => r.Status == "Failed"),
            WarningChecks = allRecommendations.Count(r => r.Status == "Warning")
        };
    }

    public async Task<List<BpaRecommendationDto>> GetRecommendationsAsync(Guid tenantId, string? severity = null, string? category = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting BPA recommendations for tenant {TenantId}", tenantId);

        var analyzersToRun = string.IsNullOrEmpty(category)
            ? _analyzers
            : _analyzers.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        var allRecommendations = new List<BpaRecommendationDto>();

        foreach (var analyzer in analyzersToRun) {
            try {
                var recommendations = await analyzer.AnalyzeAsync(tenantId, cancellationToken);
                allRecommendations.AddRange(recommendations);
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to get recommendations from analyzer {Category}", analyzer.Category);
            }
        }

        var filteredRecommendations = allRecommendations
            .Where(r => string.IsNullOrEmpty(severity) || r.Severity.Equals(severity, StringComparison.OrdinalIgnoreCase))
            .Where(r => r.Status != "Passed")
            .OrderByDescending(r => GetSeverityWeight(r.Severity))
            .ToList();

        return filteredRecommendations;
    }

    public async Task<ComplianceScoreDto> GetComplianceScoreAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Calculating compliance score for tenant {TenantId}", tenantId);

        var tenant = await _tenantService.GetTenantAsync(tenantId);
        var tenantName = tenant?.DefaultDomainName ?? "Unknown";

        var categoryBreakdown = new Dictionary<string, int>();
        var totalScore = 0;
        var maxTotalScore = 0;

        foreach (var analyzer in _analyzers) {
            try {
                var score = await analyzer.CalculateScoreAsync(tenantId, cancellationToken);
                categoryBreakdown[analyzer.Category] = analyzer.MaxScore > 0
                    ? (int)((double)score / analyzer.MaxScore * 100)
                    : 0;

                totalScore += score;
                maxTotalScore += analyzer.MaxScore;
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to calculate score for category {Category}", analyzer.Category);
            }
        }

        var percentage = maxTotalScore > 0 ? (double)totalScore / maxTotalScore * 100 : 0;
        var grade = CalculateGrade(percentage);

        return new ComplianceScoreDto {
            TenantId = tenantId,
            TenantName = tenantName,
            Score = totalScore,
            MaxScore = maxTotalScore,
            Percentage = Math.Round(percentage, 2),
            Grade = grade,
            CalculatedDate = DateTime.UtcNow,
            CategoryBreakdown = categoryBreakdown
        };
    }

    private static int GetSeverityWeight(string severity) {
        return severity?.ToLower() switch {
            "critical" => 4,
            "high" => 3,
            "medium" => 2,
            "low" => 1,
            _ => 0
        };
    }

    private static string CalculateGrade(double percentage) {
        return percentage switch {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }
}
