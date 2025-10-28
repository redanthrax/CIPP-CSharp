namespace CIPP.Api.Modules.Standards.Models;

public class PasswordPolicyConfig {
    public int? MinimumLength { get; set; }
    public bool? RequireComplexity { get; set; }
    public int? PasswordExpiryDays { get; set; }
}
