namespace CIPP.Shared.DTOs.Security;

public class UpdateSecurityAlertDto {
    public string TenantId { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? AssignedTo { get; set; }
    public string? Comments { get; set; }
}
