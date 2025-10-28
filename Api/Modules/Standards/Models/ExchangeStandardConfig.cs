namespace CIPP.Api.Modules.Standards.Models;

public class ExchangeStandardConfig {
    public string StandardName { get; set; } = string.Empty;
    public List<TransportRuleConfig>? TransportRules { get; set; }
    public List<AntiSpamPolicyConfig>? AntiSpamPolicies { get; set; }
}
