namespace CIPP.Shared.DTOs.Exchange;

public class MailboxDetailsDto : MailboxDto {
    public MailboxForwardingDto? Forwarding { get; set; }
    public List<MailboxPermissionDto> Permissions { get; set; } = new();
    public MailboxStatisticsDto? Statistics { get; set; }
    public bool? LitigationHoldEnabled { get; set; }
    public DateTime? LitigationHoldDate { get; set; }
    public string? LitigationHoldOwner { get; set; }
    public bool? RetentionHoldEnabled { get; set; }
    public DateTime? WhenCreated { get; set; }
    public DateTime? WhenChanged { get; set; }
    public List<string> GrantSendOnBehalfTo { get; set; } = new();
    public string? CustomAttribute1 { get; set; }
    public string? CustomAttribute2 { get; set; }
    public string? CustomAttribute3 { get; set; }
}
