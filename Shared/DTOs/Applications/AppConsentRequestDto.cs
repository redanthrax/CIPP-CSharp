namespace CIPP.Shared.DTOs.Applications;

public class AppConsentRequestDto {
    public string Id { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string AppDisplayName { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime? RequestDateTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> RequestedPermissions { get; set; } = new();
    public Guid? TenantId { get; set; }
}
