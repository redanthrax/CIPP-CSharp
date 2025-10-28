namespace CIPP.Api.Modules.Standards.Models;

public class SharePointStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public List<SiteTemplateConfig>? SiteTemplates { get; set; }
    public SharePointSettingsOverride? SettingsOverride { get; set; }
}
