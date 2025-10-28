namespace CIPP.Api.Modules.Standards.Models;

public class IntuneStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public List<IntunePolicyConfig>? DevicePolicies { get; set; }
    public List<IntunePolicyConfig>? CompliancePolicies { get; set; }
}
