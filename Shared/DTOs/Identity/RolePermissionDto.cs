namespace CIPP.Shared.DTOs.Identity;

public class RolePermissionDto {
    public string Id { get; set; } = string.Empty;
    public List<string> AllowedResourceActions { get; set; } = new();
    public string Condition { get; set; } = string.Empty;
}