namespace CIPP.Api.Modules.Alerts.Models;

public class AlertTemplate {
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public string? ButtonUrl { get; set; }
    public string? ButtonText { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}
