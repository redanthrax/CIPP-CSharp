namespace CIPP.Api.Modules.Standards.Models;

public class IntunePolicyConfig {
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PolicyType { get; set; } = string.Empty;
    public Dictionary<string, object>? Settings { get; set; }
}
