namespace CIPP.Shared.DTOs.Applications;

public class CreatePermissionSetDto {
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PermissionsJson { get; set; } = string.Empty;
}
