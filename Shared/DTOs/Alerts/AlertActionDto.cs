namespace CIPP.Shared.DTOs.Alerts;

public class AlertActionDto {
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public Dictionary<string, object>? Parameters { get; set; }
}
