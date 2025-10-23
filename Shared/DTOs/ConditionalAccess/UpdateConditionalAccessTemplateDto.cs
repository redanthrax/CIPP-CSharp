namespace CIPP.Shared.DTOs.ConditionalAccess;

public class UpdateConditionalAccessTemplateDto {
    public string? TemplateName { get; set; }
    public string? Description { get; set; }
    public ConditionalAccessPolicyDto? PolicyData { get; set; }
    public string? UpdatedBy { get; set; }
}
