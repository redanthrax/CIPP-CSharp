namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessTemplateDto {
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ConditionalAccessPolicyDto? PolicyData { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
