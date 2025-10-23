namespace CIPP.Shared.DTOs.Exchange;

public class UpdateMailboxDto {
    public string? DisplayName { get; set; }
    public List<string>? EmailAddresses { get; set; }
    public bool? LitigationHoldEnabled { get; set; }
    public bool? RetentionHoldEnabled { get; set; }
    public long? ProhibitSendQuota { get; set; }
    public long? ProhibitSendReceiveQuota { get; set; }
    public List<string>? GrantSendOnBehalfTo { get; set; }
    public string? CustomAttribute1 { get; set; }
    public string? CustomAttribute2 { get; set; }
    public string? CustomAttribute3 { get; set; }
}
