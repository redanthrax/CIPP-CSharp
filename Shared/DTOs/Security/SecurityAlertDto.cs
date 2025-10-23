namespace CIPP.Shared.DTOs.Security;

public class SecurityAlertDto {
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? EventDateTime { get; set; }
    public List<string> InvolvedUsers { get; set; } = new();
    public string TenantId { get; set; } = string.Empty;
}
