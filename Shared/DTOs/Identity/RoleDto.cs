namespace CIPP.Shared.DTOs.Identity;

public class RoleDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TemplateId { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; }
    public bool IsEnabled { get; set; }
    public List<string> ResourceScopes { get; set; } = new();
    public List<RolePermissionDto> RolePermissions { get; set; } = new();
    public string? TenantId { get; set; }
}