namespace CIPP.Shared.DTOs.Exchange;

public class ActiveSyncDeviceAccessRuleDto {
    public string Identity { get; set; } = string.Empty;
    public string QueryString { get; set; } = string.Empty;
    public string Characteristic { get; set; } = string.Empty;
    public string AccessLevel { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}