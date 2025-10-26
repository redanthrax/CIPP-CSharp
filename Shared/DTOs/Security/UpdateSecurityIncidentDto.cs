namespace CIPP.Shared.DTOs.Security;

public class UpdateSecurityIncidentDto {
    public Guid TenantId { get; set; }
    public string? Status { get; set; }
    public string? Classification { get; set; }
    public string? Determination { get; set; }
    public string? AssignedTo { get; set; }
    public List<string>? Tags { get; set; }
}
