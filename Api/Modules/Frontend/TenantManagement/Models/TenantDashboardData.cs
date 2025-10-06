namespace CIPP.Api.Modules.Frontend.TenantManagement.Models;

public class TenantDashboardData {
    public Guid TenantId { get; set; }
    public required string DisplayName { get; set; }
    public TenantHealthStatus HealthStatus { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalLicenses { get; set; }
    public int UsedLicenses { get; set; }
    public int ActiveAlerts { get; set; }
    public int StandardsCompliance { get; set; }
    public DateTime LastHealthCheck { get; set; }
    public PortalLinks Portals { get; set; } = new();
}

public enum TenantHealthStatus {
    Healthy,
    Warning,
    Critical,
    Unknown
}