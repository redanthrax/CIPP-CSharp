namespace CIPP.Shared.DTOs.Applications;

public class PermissionSetDto {
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PermissionsJson { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
