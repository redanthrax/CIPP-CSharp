namespace CIPP.Shared.DTOs.Exchange;

public class MailboxPermissionDto {
    public string User { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public List<string> AccessRights { get; set; } = new();
    public string PermissionType { get; set; } = string.Empty;
}
