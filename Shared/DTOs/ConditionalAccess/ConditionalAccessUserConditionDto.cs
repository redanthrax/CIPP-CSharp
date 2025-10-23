namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessUserConditionDto {
    public List<string>? IncludeUsers { get; set; }
    public List<string>? ExcludeUsers { get; set; }
    public List<string>? IncludeGroups { get; set; }
    public List<string>? ExcludeGroups { get; set; }
    public List<string>? IncludeRoles { get; set; }
    public List<string>? ExcludeRoles { get; set; }
}
