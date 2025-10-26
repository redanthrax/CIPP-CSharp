namespace CIPP.Shared.DTOs.Applications;

public class UpdateApplicationDto {
    public Guid TenantId { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? SignInAudience { get; set; }
    public List<string>? RedirectUris { get; set; }
    public string? Homepage { get; set; }
    public string? LogoutUrl { get; set; }
    public List<string>? Tags { get; set; }
    public string? Notes { get; set; }
}
