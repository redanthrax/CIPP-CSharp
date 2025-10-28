namespace CIPP.Shared.DTOs.Exchange;

public class MobileDeviceStatisticsDto {
    public string Identity { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string DeviceModel { get; set; } = string.Empty;
    public string DeviceOS { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public DateTime? FirstSyncTime { get; set; }
    public DateTime? LastSuccessSync { get; set; }
    public DateTime? LastPolicyUpdateTime { get; set; }
    public DateTime? LastPingHeartbeat { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? NumberOfFoldersSynced { get; set; }
    public long? TotalItemsInMailbox { get; set; }
    public long? TotalItemsSyncedFromMailbox { get; set; }
    public Guid TenantId { get; set; }
}
