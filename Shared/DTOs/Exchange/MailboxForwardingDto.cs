namespace CIPP.Shared.DTOs.Exchange;

public class MailboxForwardingDto {
    public string? ForwardingAddress { get; set; }
    public string? ForwardingSmtpAddress { get; set; }
    public bool DeliverToMailboxAndForward { get; set; }
}
