namespace CIPP.Shared.DTOs.Exchange;

public class TransportRuleDetailsDto : TransportRuleDto {
    public TransportRuleConditionsDto Conditions { get; set; } = new();
    public TransportRuleActionsDto Actions { get; set; } = new();
    public DateTime? ActivationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? RuleErrorAction { get; set; }
    public string? SenderAddressLocation { get; set; }
}
