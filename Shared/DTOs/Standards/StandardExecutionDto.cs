namespace CIPP.Shared.DTOs.Standards;

public class StandardExecutionDto {
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime ExecutedDate { get; set; }
    public string? ExecutedBy { get; set; }
    public int? DurationMs { get; set; }
}
