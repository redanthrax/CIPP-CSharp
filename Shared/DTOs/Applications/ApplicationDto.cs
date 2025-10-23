namespace CIPP.Shared.DTOs.Applications;

public class ApplicationDto {
    public string Id { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedDateTime { get; set; }
    public string SignInAudience { get; set; } = string.Empty;
    public List<string> RedirectUris { get; set; } = new();
    public string? PublisherDomain { get; set; }
    public string? TenantId { get; set; }
    public bool? IsEnabled { get; set; }
    public List<ApplicationCredentialDto> PasswordCredentials { get; set; } = new();
    public List<ApplicationCredentialDto> KeyCredentials { get; set; } = new();
}
