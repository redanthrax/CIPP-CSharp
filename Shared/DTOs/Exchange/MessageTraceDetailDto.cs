namespace CIPP.Shared.DTOs.Exchange;

public class MessageTraceDetailDto {
    public string MessageId { get; set; } = string.Empty;
    public string MessageTraceId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Event { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string? Data { get; set; }
}
