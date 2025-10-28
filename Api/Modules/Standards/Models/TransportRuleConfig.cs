namespace CIPP.Api.Modules.Standards.Models;

public class TransportRuleConfig {
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, object> Settings { get; set; } = new();
}
