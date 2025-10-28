namespace CIPP.Api.Modules.Standards.Models;

public class SiteTemplateConfig {
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Template { get; set; } = "CommunicationSite";
    public string? Owner { get; set; }
    public Dictionary<string, object>? AdditionalSettings { get; set; }
}
