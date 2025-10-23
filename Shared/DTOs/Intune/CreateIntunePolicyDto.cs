namespace CIPP.Shared.DTOs.Intune;

public class CreateIntunePolicyDto {
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TemplateType { get; set; } = string.Empty;
    public string RawJson { get; set; } = string.Empty;
    public string? AssignTo { get; set; }
    public string? ExcludeGroup { get; set; }
}
