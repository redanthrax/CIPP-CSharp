namespace CIPP.Shared.DTOs.ConditionalAccess;

public class DeployConditionalAccessTemplateDto {
    public Guid TemplateId { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string? State { get; set; }
    public bool Overwrite { get; set; } = false;
}
