namespace CIPP.Shared.DTOs.Identity;

public class UpdateAuthenticationMethodDto {
    public string State { get; set; } = string.Empty;
    public bool? IsAttestationEnforced { get; set; }
    public bool? IsSelfServiceRegistrationAllowed { get; set; }
    public bool? IsSoftwareOathEnabled { get; set; }
    public int? MinimumLifetimeInMinutes { get; set; }
    public int? MaximumLifetimeInMinutes { get; set; }
    public int? DefaultLifetimeInMinutes { get; set; }
    public int? DefaultLength { get; set; }
    public bool? IsUsableOnce { get; set; }
}
