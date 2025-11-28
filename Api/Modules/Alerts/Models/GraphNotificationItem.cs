namespace CIPP.Api.Modules.Alerts.Models;

public class GraphNotificationItem {
    public string? SubscriptionId { get; set; }
    public string? ClientState { get; set; }
    public string? ChangeType { get; set; }
    public string? Resource { get; set; }
    public DateTime? SubscriptionExpirationDateTime { get; set; }
    public object? ResourceData { get; set; }
}
