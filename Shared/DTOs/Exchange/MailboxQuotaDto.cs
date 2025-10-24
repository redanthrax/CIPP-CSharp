namespace CIPP.Shared.DTOs.Exchange;

public class MailboxQuotaDto {
    public string? IssueWarningQuota { get; set; }
    public string? ProhibitSendQuota { get; set; }
    public string? ProhibitSendReceiveQuota { get; set; }
    public bool UseDatabaseQuotaDefaults { get; set; }
}
