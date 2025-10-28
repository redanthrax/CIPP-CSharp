namespace CIPP.Shared.DTOs.Exchange;

public class MailboxAutoReplyConfigurationDto {
    public string Identity { get; set; } = string.Empty;
    public string? AutoReplyState { get; set; }
    public string? InternalMessage { get; set; }
    public string? ExternalMessage { get; set; }
    public bool? ExternalAudience { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
