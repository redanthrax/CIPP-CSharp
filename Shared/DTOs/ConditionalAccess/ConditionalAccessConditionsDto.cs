namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessConditionsDto {
    public ConditionalAccessUserConditionDto? Users { get; set; }
    public ConditionalAccessApplicationConditionDto? Applications { get; set; }
    public List<string>? ClientAppTypes { get; set; }
    public ConditionalAccessLocationConditionDto? Locations { get; set; }
    public ConditionalAccessPlatformConditionDto? Platforms { get; set; }
    public ConditionalAccessDeviceConditionDto? Devices { get; set; }
    public List<string>? SignInRiskLevels { get; set; }
    public List<string>? UserRiskLevels { get; set; }
}
