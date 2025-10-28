namespace CIPP.Shared.DTOs.Tenants;

public class EditTenantRequestDto
{
    public Guid TenantId { get; set; }
    public string? TenantAlias { get; set; }
    public List<Guid>? TenantGroups { get; set; }
    public Dictionary<string, object>? CustomVariables { get; set; }
    public string? OffboardingDefaults { get; set; }

    public EditTenantRequestDto() { }

    public EditTenantRequestDto(Guid tenantId, string? tenantAlias, List<Guid>? tenantGroups,
        Dictionary<string, object>? customVariables, string? offboardingDefaults)
    {
        TenantId = tenantId;
        TenantAlias = tenantAlias;
        TenantGroups = tenantGroups;
        CustomVariables = customVariables;
        OffboardingDefaults = offboardingDefaults;
    }
}
