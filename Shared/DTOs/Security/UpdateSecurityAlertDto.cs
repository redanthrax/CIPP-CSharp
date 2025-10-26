namespace CIPP.Shared.DTOs.Security;

public class UpdateSecurityAlertDto {
    public Guid TenantId { get; set; }
    public string? Status { get; set; }
    public string? AssignedTo { get; set; }
    public string? Comments { get; set; }
}
