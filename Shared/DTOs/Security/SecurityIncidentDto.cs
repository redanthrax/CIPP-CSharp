namespace CIPP.Shared.DTOs.Security;

public class SecurityIncidentDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string? Classification { get; set; }
    public string? Determination { get; set; }
    public string IncidentUrl { get; set; } = string.Empty;
    public string? RedirectId { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? LastUpdateDateTime { get; set; }
    public List<string> Tags { get; set; } = new();
    public Guid TenantId { get; set; }
}
