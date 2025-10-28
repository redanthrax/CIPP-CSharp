namespace CIPP.Shared.DTOs.Exchange;

public class CreateActiveSyncDeviceAccessRuleDto {
    public string QueryString { get; set; } = string.Empty;
    public string Characteristic { get; set; } = string.Empty;
    public string AccessLevel { get; set; } = string.Empty;
}