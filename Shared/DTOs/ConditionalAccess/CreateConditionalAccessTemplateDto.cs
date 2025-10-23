namespace CIPP.Shared.DTOs.ConditionalAccess;

public class CreateConditionalAccessTemplateDto {
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ConditionalAccessPolicyDto PolicyData { get; set; } = new();
    public string? CreatedBy { get; set; }
}
