namespace CIPP.Api.Modules.Standards.Models;

public class TeamsStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public List<TeamTemplateConfig>? TeamTemplates { get; set; }
    public TeamsPolicySettings? PolicySettings { get; set; }
}
