namespace CIPP.Shared.DTOs.ConditionalAccess;

public class CreateConditionalAccessPolicyDto {
    public Guid TenantId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string State { get; set; } = "disabled";
    
    public ConditionalAccessConditionsDto? Conditions { get; set; }
    public ConditionalAccessGrantControlsDto? GrantControls { get; set; }
    public ConditionalAccessSessionControlsDto? SessionControls { get; set; }
}
