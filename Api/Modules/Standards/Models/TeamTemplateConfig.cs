namespace CIPP.Api.Modules.Standards.Models;

public class TeamTemplateConfig {
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Visibility { get; set; } = "Private";
    public string Template { get; set; } = "standard";
    public List<string>? Members { get; set; }
    public List<string>? Owners { get; set; }
}
