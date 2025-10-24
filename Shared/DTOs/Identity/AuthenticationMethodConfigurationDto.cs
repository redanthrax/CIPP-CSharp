namespace CIPP.Shared.DTOs.Identity;

public class AuthenticationMethodConfigurationDto {
    public string Id { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public AuthenticationMethodTargetDto? IncludeTargets { get; set; }
    public AuthenticationMethodTargetDto? ExcludeTargets { get; set; }
}
