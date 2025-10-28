namespace CIPP.Shared.DTOs.Exchange;

public class MobileDeviceDto {
    public string Identity { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;
    public string DeviceModel { get; set; } = string.Empty;
    public string DeviceOS { get; set; } = string.Empty;
    public string DeviceUserAgent { get; set; } = string.Empty;
    public string DeviceImei { get; set; } = string.Empty;
    public string DeviceMobileOperator { get; set; } = string.Empty;
    public string DeviceTelephoneNumber { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public string DeviceFriendlyName { get; set; } = string.Empty;
    public DateTime? FirstSyncTime { get; set; }
    public DateTime? LastSuccessSync { get; set; }
    public DateTime? LastPolicyUpdateTime { get; set; }
    public DateTime? LastPingHeartbeat { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusNote { get; set; } = string.Empty;
    public bool DeviceAccessState { get; set; }
    public string DeviceAccessStateReason { get; set; } = string.Empty;
    public string DeviceAccessControlRule { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}
