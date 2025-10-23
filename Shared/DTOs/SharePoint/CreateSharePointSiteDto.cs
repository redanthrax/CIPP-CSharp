namespace CIPP.Shared.DTOs.SharePoint;

public class CreateSharePointSiteDto {
    public string SiteName { get; set; } = string.Empty;
    public string? SiteDescription { get; set; }
    public string SiteOwner { get; set; } = string.Empty;
    public string? TemplateName { get; set; }
    public string? SiteDesign { get; set; }
    public string? SensitivityLabel { get; set; }
}
