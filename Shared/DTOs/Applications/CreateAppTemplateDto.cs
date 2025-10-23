namespace CIPP.Shared.DTOs.Applications;

public class CreateAppTemplateDto {
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TemplateType { get; set; } = string.Empty;
    public string TemplateJson { get; set; } = string.Empty;
}
