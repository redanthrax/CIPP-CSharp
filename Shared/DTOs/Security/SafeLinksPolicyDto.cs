namespace CIPP.Shared.DTOs.Security;

public class SafeLinksPolicyDto {
    public string Name { get; set; } = string.Empty;
    public string? AdminDisplayName { get; set; }
    public bool? EnableSafeLinksForEmail { get; set; }
    public bool? EnableSafeLinksForTeams { get; set; }
    public bool? EnableSafeLinksForOffice { get; set; }
    public bool? TrackClicks { get; set; }
    public bool? AllowClickThrough { get; set; }
    public bool? ScanUrls { get; set; }
    public bool? EnableForInternalSenders { get; set; }
    public bool? DeliverMessageAfterScan { get; set; }
    public bool? DisableUrlRewrite { get; set; }
    public List<string>? DoNotRewriteUrls { get; set; }
    public string? CustomNotificationText { get; set; }
    public bool? EnableOrganizationBranding { get; set; }
    public string? PolicyName { get; set; }
    public string? RuleName { get; set; }
    public int? Priority { get; set; }
    public string? State { get; set; }
    public List<string>? SentTo { get; set; }
    public List<string>? SentToMemberOf { get; set; }
    public List<string>? RecipientDomainIs { get; set; }
    public List<string>? ExceptIfSentTo { get; set; }
    public List<string>? ExceptIfSentToMemberOf { get; set; }
    public List<string>? ExceptIfRecipientDomainIs { get; set; }
    public string? Description { get; set; }
    public bool IsBuiltIn { get; set; }
    public bool? IsValid { get; set; }
    public string? ConfigurationStatus { get; set; }
}
