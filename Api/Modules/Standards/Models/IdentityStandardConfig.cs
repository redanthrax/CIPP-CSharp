namespace CIPP.Api.Modules.Standards.Models;

public class IdentityStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public MfaSettingsConfig? MfaSettings { get; set; }
    public PasswordPolicyConfig? PasswordPolicy { get; set; }
}
