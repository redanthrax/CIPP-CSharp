namespace CIPP.Shared.DTOs.Tenants;

public class EditTenantRequestDto
{
    public Guid Id { get; set; }
    public string? TenantAlias { get; set; }
    public List<Guid>? TenantGroups { get; set; }
    public Dictionary<string, object>? CustomVariables { get; set; }
    public string? OffboardingDefaults { get; set; }

    public EditTenantRequestDto() { }

    public EditTenantRequestDto(Guid id, string? tenantAlias, List<Guid>? tenantGroups,
        Dictionary<string, object>? customVariables, string? offboardingDefaults)
    {
        Id = id;
        TenantAlias = tenantAlias;
        TenantGroups = tenantGroups;
        CustomVariables = customVariables;
        OffboardingDefaults = offboardingDefaults;
    }
}
