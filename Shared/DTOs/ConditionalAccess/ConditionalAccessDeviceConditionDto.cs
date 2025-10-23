namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessDeviceConditionDto {
    public List<string>? IncludeDeviceStates { get; set; }
    public List<string>? ExcludeDeviceStates { get; set; }
}
