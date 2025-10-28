namespace CIPP.Api.Modules.Standards.Models;

public class SecurityStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public List<SecureScoreControlConfig>? SecureScoreControls { get; set; }
    public Dictionary<string, object>? DefenderSettings { get; set; }
}
