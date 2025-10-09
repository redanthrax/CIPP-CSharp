namespace CIPP.Shared.DTOs.Alerts;

public class AlertCommandDto {
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequiresInput { get; set; }
    public string? InputType { get; set; }
    public string? InputLabel { get; set; }
    public string? InputPlaceholder { get; set; }
    public string? InputName { get; set; }
    public string? RecommendedRunInterval { get; set; }
}