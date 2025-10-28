namespace CIPP.Shared.DTOs.Standards;

public class BpaCategoryScoreDto {
    public string Category { get; set; } = string.Empty;
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public int ChecksPassed { get; set; }
    public int ChecksFailed { get; set; }
    public int ChecksWarning { get; set; }
}
