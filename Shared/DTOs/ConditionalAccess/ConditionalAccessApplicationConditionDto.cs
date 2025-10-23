namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessApplicationConditionDto {
    public List<string>? IncludeApplications { get; set; }
    public List<string>? ExcludeApplications { get; set; }
    public List<string>? IncludeUserActions { get; set; }
}
