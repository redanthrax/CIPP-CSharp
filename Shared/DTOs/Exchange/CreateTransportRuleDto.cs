namespace CIPP.Shared.DTOs.Exchange;

public class CreateTransportRuleDto {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; } = 0;
    public bool Enabled { get; set; } = true;
    public string Mode { get; set; } = "Enforce";
    public string? Comments { get; set; }
    public TransportRuleConditionsDto Conditions { get; set; } = new();
    public TransportRuleActionsDto Actions { get; set; } = new();
    public DateTime? ActivationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
