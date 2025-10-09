using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Shared.DTOs.Alerts;

public class CreateScriptedAlertDto {
    public string? Name { get; set; }
    public string? LogType { get; set; }
    public List<TenantSelectorOptionDto> TenantFilter { get; set; } = new();
    public List<TenantSelectorOptionDto>? ExcludedTenants { get; set; } = new();
    public List<AlertConditionDto> Conditions { get; set; } = new();
    public List<AlertActionDto> Actions { get; set; } = new();
    public string? ScheduleCron { get; set; } 
}