namespace CIPP.Shared.DTOs.Standards;

public class BpaRecommendationDto {
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium";
    public string Status { get; set; } = "Failed";
    public string? RemediationSteps { get; set; }
    public string? StandardType { get; set; }
    public string? RelatedControl { get; set; }
}
