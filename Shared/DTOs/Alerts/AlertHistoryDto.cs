namespace CIPP.Shared.DTOs.Alerts;

public class AlertHistoryDto {
    public Guid Id { get; set; }
    public string TenantFilter { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string LogType { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string RawData { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? WebhookRuleId { get; set; }
    public string? RuleName { get; set; }
}
