namespace CIPP.Shared.DTOs.Exchange;

public class MessageTraceDto {
    public string MessageId { get; set; } = string.Empty;
    public string MessageTraceId { get; set; } = string.Empty;
    public DateTime Received { get; set; }
    public string SenderAddress { get; set; } = string.Empty;
    public List<string> RecipientAddress { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ToIP { get; set; } = string.Empty;
    public string FromIP { get; set; } = string.Empty;
    public int Size { get; set; }
    public string? Organization { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
