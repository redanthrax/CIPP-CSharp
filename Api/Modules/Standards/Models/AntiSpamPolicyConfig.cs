namespace CIPP.Api.Modules.Standards.Models;

public class AntiSpamPolicyConfig {
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, object> Settings { get; set; } = new();
}
