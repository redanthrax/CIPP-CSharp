namespace CIPP.Shared.DTOs.Exchange;

public class TransportRuleDto {
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; }
    public bool Enabled { get; set; }
    public string State { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public DateTime? WhenChanged { get; set; }
    public string? Comments { get; set; }
}
