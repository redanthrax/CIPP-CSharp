namespace CIPP.Shared.DTOs.Exchange;

public class HostedContentFilterPolicyDto {
    public string Identity { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsValid { get; set; }
    public string? AdminDisplayName { get; set; }
    public int BulkThreshold { get; set; }
    public List<string> AllowedSenders { get; set; } = new();
    public List<string> AllowedSenderDomains { get; set; } = new();
    public List<string> BlockedSenders { get; set; } = new();
    public List<string> BlockedSenderDomains { get; set; } = new();
    public bool EnableEndUserSpamNotifications { get; set; }
    public int EndUserSpamNotificationFrequency { get; set; }
    public string? SpamAction { get; set; }
    public string? HighConfidenceSpamAction { get; set; }
    public string? PhishSpamAction { get; set; }
    public string? HighConfidencePhishAction { get; set; }
    public string? BulkSpamAction { get; set; }
    public bool MarkAsSpamBulkMail { get; set; }
    public bool IncreaseScoreWithImageLinks { get; set; }
    public bool IncreaseScoreWithNumericIps { get; set; }
    public bool IncreaseScoreWithRedirectToOtherPort { get; set; }
    public bool IncreaseScoreWithBizOrInfoUrls { get; set; }
    public int QuarantineRetentionPeriod { get; set; }
    public DateTime? WhenChanged { get; set; }
}
