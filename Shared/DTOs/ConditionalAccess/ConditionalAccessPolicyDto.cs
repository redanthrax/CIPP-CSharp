namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessPolicyDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string State { get; set; } = string.Empty;
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public Guid TenantId { get; set; }
    
    public ConditionalAccessConditionsDto? Conditions { get; set; }
    public ConditionalAccessGrantControlsDto? GrantControls { get; set; }
    public ConditionalAccessSessionControlsDto? SessionControls { get; set; }
}
