namespace CIPP.Shared.DTOs.Exchange;

public class UpdateMailboxForwardingDto {
    public string? ForwardingAddress { get; set; }
    public string? ForwardingSmtpAddress { get; set; }
    public bool DeliverToMailboxAndForward { get; set; }
    public bool DisableForwarding { get; set; }
}
