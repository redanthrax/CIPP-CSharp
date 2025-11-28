using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Shared.DTOs.Alerts;

public class AlertConfigurationDto {
    public string RowKey { get; set; } = string.Empty;
    public string PartitionKey { get; set; } = string.Empty;
    public List<TenantSelectorOptionDto> Tenants { get; set; } = new();
    public List<string> ExcludedTenants { get; set; } = new();
    public string Conditions { get; set; } = string.Empty;
    public string Actions { get; set; } = string.Empty;
    public string LogType { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string RepeatsEvery { get; set; } = string.Empty;
    public string AlertComment { get; set; } = string.Empty;
    public object? RawAlert { get; set; }
}
