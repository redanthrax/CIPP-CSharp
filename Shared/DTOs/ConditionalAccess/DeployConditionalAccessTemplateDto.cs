namespace CIPP.Shared.DTOs.ConditionalAccess;

public class DeployConditionalAccessTemplateDto {
    public Guid TemplateId { get; set; }
    public Guid TenantId { get; set; }
    public string? State { get; set; }
    public bool Overwrite { get; set; } = false;
}
