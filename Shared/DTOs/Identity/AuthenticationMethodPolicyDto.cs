namespace CIPP.Shared.DTOs.Identity;

public class AuthenticationMethodPolicyDto {
    public string Id { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public List<AuthenticationMethodConfigurationDto> AuthenticationMethodConfigurations { get; set; } = new();
}
