namespace CIPP.Shared.DTOs.Alerts;

public class AlertPropertyDto {
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool Multi { get; set; } = false;
}