namespace CIPP.Shared.DTOs.Exchange;

public class MessageTraceSearchDto {
    public string? SenderAddress { get; set; }
    public string? RecipientAddress { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? MessageId { get; set; }
    public string? Subject { get; set; }
    public string? Status { get; set; }
    public string? FromIP { get; set; }
    public string? ToIP { get; set; }
}
