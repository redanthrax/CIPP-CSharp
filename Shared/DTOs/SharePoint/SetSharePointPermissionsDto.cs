namespace CIPP.Shared.DTOs.SharePoint;

public class SetSharePointPermissionsDto {
    public string UserId { get; set; } = string.Empty;
    public string AccessUser { get; set; } = string.Empty;
    public string? Url { get; set; }
    public bool RemovePermission { get; set; }
}
