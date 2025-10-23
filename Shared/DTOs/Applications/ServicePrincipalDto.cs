namespace CIPP.Shared.DTOs.Applications;

public class ServicePrincipalDto {
    public string Id { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool AccountEnabled { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string ServicePrincipalType { get; set; } = string.Empty;
    public List<string> ServicePrincipalNames { get; set; } = new();
    public string? PublisherName { get; set; }
    public string? HomePage { get; set; }
    public string? TenantId { get; set; }
    public List<string> ReplyUrls { get; set; } = new();
    public List<ApplicationCredentialDto> PasswordCredentials { get; set; } = new();
    public List<ApplicationCredentialDto> KeyCredentials { get; set; } = new();
}
