namespace CIPP.Shared.DTOs.Exchange;

public class UpdateMailboxPermissionsDto {
    public string User { get; set; } = string.Empty;
    public List<string> AccessRights { get; set; } = new();
    public string PermissionType { get; set; } = string.Empty;
    public bool RemovePermission { get; set; }
}
