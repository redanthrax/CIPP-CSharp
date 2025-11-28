namespace CIPP.Shared.DTOs.Alerts;

public class AlertCommandValueDto {
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequiresInput { get; set; } = false;
    public string? InputType { get; set; }
    public string? InputName { get; set; }
    public string? InputLabel { get; set; }
    public string? RecommendedRunInterval { get; set; }
}
