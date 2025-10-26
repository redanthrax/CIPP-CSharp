namespace CIPP.Shared.DTOs.Applications;

public class DeleteApplicationCredentialDto {
    public Guid TenantId { get; set; }
    public string ApplicationId { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
}
