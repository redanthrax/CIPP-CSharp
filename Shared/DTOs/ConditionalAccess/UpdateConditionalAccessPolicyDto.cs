namespace CIPP.Shared.DTOs.ConditionalAccess;

public class UpdateConditionalAccessPolicyDto {
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? State { get; set; }
    
    public ConditionalAccessConditionsDto? Conditions { get; set; }
    public ConditionalAccessGrantControlsDto? GrantControls { get; set; }
    public ConditionalAccessSessionControlsDto? SessionControls { get; set; }
}
