namespace CIPP.Shared.DTOs.Identity;

public class AuthenticationMethodDto {
    public string Id { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public List<AuthenticationMethodTargetDto> IncludeTargets { get; set; } = new();
    public List<AuthenticationMethodTargetDto> ExcludeTargets { get; set; } = new();
}
