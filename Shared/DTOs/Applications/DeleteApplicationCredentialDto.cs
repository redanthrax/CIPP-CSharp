namespace CIPP.Shared.DTOs.Applications;

public class DeleteApplicationCredentialDto {
    public string TenantId { get; set; } = string.Empty;
    public string ApplicationId { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
}
