namespace CIPP.Shared.DTOs.Exchange;

public class MailboxDto {
    public string Id { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public string MailboxType { get; set; } = string.Empty;
    public List<string> EmailAddresses { get; set; } = new();
    public bool? IsMailboxEnabled { get; set; }
    public string? ExchangeGuid { get; set; }
    public string? ArchiveGuid { get; set; }
    public bool? ArchiveStatus { get; set; }
    public long? ProhibitSendQuota { get; set; }
    public long? ProhibitSendReceiveQuota { get; set; }
    public string? RecipientType { get; set; }
    public string? RecipientTypeDetails { get; set; }
}
