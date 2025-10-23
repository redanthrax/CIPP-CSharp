namespace CIPP.Shared.DTOs.Exchange;

public class UpdateTransportRuleDto {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public bool? Enabled { get; set; }
    public string? Mode { get; set; }
    public string? Comments { get; set; }
    public TransportRuleConditionsDto? Conditions { get; set; }
    public TransportRuleActionsDto? Actions { get; set; }
    public DateTime? ActivationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
