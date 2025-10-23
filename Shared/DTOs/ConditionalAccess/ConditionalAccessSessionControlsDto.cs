namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessSessionControlsDto {
    public bool? DisableResilienceDefaults { get; set; }
    public bool? ApplicationEnforcedRestrictionsIsEnabled { get; set; }
    public bool? CloudAppSecurityIsEnabled { get; set; }
    public string? CloudAppSecurityType { get; set; }
    public bool? SignInFrequencyIsEnabled { get; set; }
    public int? SignInFrequencyValue { get; set; }
    public string? SignInFrequencyType { get; set; }
    public bool? PersistentBrowserIsEnabled { get; set; }
    public string? PersistentBrowserMode { get; set; }
}
