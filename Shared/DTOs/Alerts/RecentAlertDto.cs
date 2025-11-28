namespace CIPP.Shared.DTOs.Alerts;

public class RecentAlertDto {
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TenantFilter { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
