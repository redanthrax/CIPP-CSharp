namespace CIPP.Shared.DTOs.Alerts;

public class AlertInputDto {
    public string? Value { get; set; }
    public string? Label { get; set; }
    public bool Multi { get; set; } = false;
}