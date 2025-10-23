namespace CIPP.Shared.DTOs.Applications;

public class CreateApplicationCredentialDto {
    public string TenantId { get; set; } = string.Empty;
    public string ApplicationId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Type { get; set; } = "Password";
    public int DurationInMonths { get; set; } = 12;
}
