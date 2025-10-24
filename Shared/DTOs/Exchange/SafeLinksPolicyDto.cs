namespace CIPP.Shared.DTOs.Exchange;

public class SafeLinksPolicyDto {
    public string Name { get; set; } = string.Empty;
    public bool EnableSafeLinksForEmail { get; set; }
    public bool EnableSafeLinksForTeams { get; set; }
    public bool EnableSafeLinksForOffice { get; set; }
    public bool TrackClicks { get; set; }
    public bool AllowClickThrough { get; set; }
    public bool ScanUrls { get; set; }
    public bool EnableForInternalSenders { get; set; }
    public bool DeliverMessageAfterScan { get; set; }
    public bool DisableUrlRewrite { get; set; }
    public bool EnableOrganizationBranding { get; set; }
    public List<string> DoNotRewriteUrls { get; set; } = new();
}
