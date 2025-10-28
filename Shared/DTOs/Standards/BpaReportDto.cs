namespace CIPP.Shared.DTOs.Standards;

public class BpaReportDto {
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public int OverallScore { get; set; }
    public DateTime GeneratedDate { get; set; }
    public List<BpaCategoryScoreDto> CategoryScores { get; set; } = new();
    public List<BpaRecommendationDto> Recommendations { get; set; } = new();
    public int TotalChecks { get; set; }
    public int PassedChecks { get; set; }
    public int FailedChecks { get; set; }
    public int WarningChecks { get; set; }
}
