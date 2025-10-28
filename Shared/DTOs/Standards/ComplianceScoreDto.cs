namespace CIPP.Shared.DTOs.Standards;

public class ComplianceScoreDto {
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public double Percentage { get; set; }
    public string Grade { get; set; } = string.Empty;
    public DateTime CalculatedDate { get; set; }
    public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
}
