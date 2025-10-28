namespace CIPP.Shared.DTOs.Standards;

public class DeployStandardDto {
    public Guid TemplateId { get; set; }
    public Guid[] TenantIds { get; set; } = Array.Empty<Guid>();
    public bool OverrideConfiguration { get; set; }
    public string? ConfigurationOverride { get; set; }
}
