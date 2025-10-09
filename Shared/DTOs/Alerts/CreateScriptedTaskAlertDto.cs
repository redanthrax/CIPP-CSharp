using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Shared.DTOs.Alerts;

public class CreateScriptedTaskAlertDto {
    public string? RowKey { get; set; }
    public TenantSelectorOptionDto? TenantFilter { get; set; }
    public List<TenantSelectorOptionDto> ExcludedTenants { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public long ScheduledTime { get; set; }
    public long? DesiredStartTime { get; set; }
    public string Recurrence { get; set; } = string.Empty;
    public string PostExecution { get; set; } = string.Empty;
}