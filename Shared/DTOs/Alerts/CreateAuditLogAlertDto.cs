using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Shared.DTOs.Alerts;

public class CreateAuditLogAlertDto {
    public string? RowKey { get; set; }
    public List<TenantSelectorOptionDto> TenantFilter { get; set; } = new();
    public List<TenantSelectorOptionDto> ExcludedTenants { get; set; } = new();
    public List<AlertConditionDto> Conditions { get; set; } = new();
    public List<AlertActionDto> Actions { get; set; } = new();
    public AlertLogbookDto? Logbook { get; set; }
    public string? AlertComment { get; set; }
}
