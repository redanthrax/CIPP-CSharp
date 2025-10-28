using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Analyzers;

public class SecurityBpaAnalyzer : IBpaAnalyzer {
    private readonly ISecureScoreService _secureScoreService;
    private readonly ILogger<SecurityBpaAnalyzer> _logger;

    public string Category => "Security";
    public int MaxScore => 100;

    public SecurityBpaAnalyzer(ISecureScoreService secureScoreService, ILogger<SecurityBpaAnalyzer> logger) {
        _secureScoreService = secureScoreService;
        _logger = logger;
    }

    public async Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var controls = await _secureScoreService.GetControlProfilesAsync(tenantId, cancellationToken);

            foreach (var control in controls) {
                var status = DetermineControlStatus(control.State ?? string.Empty);
                var severity = DetermineSeverity(control.Rank ?? string.Empty, control.MaxScore ?? 0);

                recommendations.Add(new BpaRecommendationDto {
                    Category = Category,
                    Title = control.Title ?? "Unknown Control",
                    Description = $"{control.Id}: {control.ControlCategory}",
                    Severity = severity,
                    Status = status,
                    RemediationSteps = control.RemediationImpact,
                    StandardType = "Security",
                    RelatedControl = control.Id
                });
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to analyze security controls for tenant {TenantId}", tenantId);
        }

        return recommendations;
    }

    public async Task<int> CalculateScoreAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        try {
            var controls = await _secureScoreService.GetControlProfilesAsync(tenantId, cancellationToken);
            var implementedControls = controls.Count(c => c.State == "Reviewed" || c.State == "Completed");
            var totalControls = controls.Count;

            return totalControls > 0 ? (int)((double)implementedControls / totalControls * MaxScore) : 0;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to calculate security score for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private static string DetermineControlStatus(string implementationStatus) {
        return implementationStatus switch {
            "Implemented" => "Passed",
            "Alternative" => "Passed",
            "Planned" => "Warning",
            _ => "Failed"
        };
    }

    private static string DetermineSeverity(string rank, int maxScore) {
        if (rank == "High" || maxScore >= 10) return "High";
        if (rank == "Medium" || maxScore >= 5) return "Medium";
        return "Low";
    }
}
