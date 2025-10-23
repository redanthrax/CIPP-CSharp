namespace CIPP.Shared.DTOs.Applications;

public class UpdatePermissionSetDto {
    public string? TemplateName { get; set; }
    public string? Description { get; set; }
    public string? PermissionsJson { get; set; }
}
