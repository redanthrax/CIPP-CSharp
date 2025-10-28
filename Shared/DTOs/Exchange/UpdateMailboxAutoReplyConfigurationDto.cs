namespace CIPP.Shared.DTOs.Exchange;

public class UpdateMailboxAutoReplyConfigurationDto {
    public string? AutoReplyState { get; set; }
    public string? InternalMessage { get; set; }
    public string? ExternalMessage { get; set; }
    public string? ExternalAudience { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
