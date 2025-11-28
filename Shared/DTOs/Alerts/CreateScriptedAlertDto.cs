using CIPP.Shared.DTOs.Tenants;
using System.Text.Json.Serialization;

namespace CIPP.Shared.DTOs.Alerts;

public class CreateScriptedAlertDto {
    public string? RowKey { get; set; }
    public TenantSelectorOptionDto? TenantFilter { get; set; }
    public List<string>? ExcludedTenants { get; set; } = new();
    public string? Name { get; set; }
    public AlertCommandDto? Command { get; set; }
    public Dictionary<string, object>? Parameters { get; set; }
    public long? ScheduledTime { get; set; }
    public string? DesiredStartTime { get; set; }
    public RecurrenceOptionDto? Recurrence { get; set; }
    public List<PostExecutionOptionDto>? PostExecution { get; set; } = new();
    public string? AlertComment { get; set; }
}
