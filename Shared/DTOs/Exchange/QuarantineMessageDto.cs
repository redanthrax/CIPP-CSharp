namespace CIPP.Shared.DTOs.Exchange;

public class QuarantineMessageDto {
    public string Identity { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string SenderAddress { get; set; } = string.Empty;
    public List<string> RecipientAddress { get; set; } = new();
    public DateTime ReceivedTime { get; set; }
    public DateTime Expires { get; set; }
    public string QuarantineTypes { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string PolicyType { get; set; } = string.Empty;
    public string? PolicyName { get; set; }
    public int Size { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool Released { get; set; }
}
