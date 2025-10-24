namespace CIPP.Shared.DTOs.Identity;

public class AuthenticationMethodTargetDto {
    public string TargetType { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public bool IsRegistrationRequired { get; set; }
}
