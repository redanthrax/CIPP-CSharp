namespace CIPP.Shared.DTOs.Applications;

public class UpdateServicePrincipalDto {
    public string TenantId { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public bool? AccountEnabled { get; set; }
    public string? Homepage { get; set; }
    public string? LogoutUrl { get; set; }
    public List<string>? Tags { get; set; }
    public string? Notes { get; set; }
    public string? PreferredSingleSignOnMode { get; set; }
}
