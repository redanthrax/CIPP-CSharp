using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Analyzers;

public class ExchangeBpaAnalyzer : IBpaAnalyzer {
    private readonly ITransportRuleService _transportRuleService;
    private readonly ISpamFilterService _spamFilterService;
    private readonly ILogger<ExchangeBpaAnalyzer> _logger;

    public string Category => "Exchange";
    public int MaxScore => 100;

    public ExchangeBpaAnalyzer(ITransportRuleService transportRuleService, ISpamFilterService spamFilterService, ILogger<ExchangeBpaAnalyzer> logger) {
        _transportRuleService = transportRuleService;
        _spamFilterService = spamFilterService;
        _logger = logger;
    }

    public async Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            recommendations.AddRange(await CheckTransportRulesAsync(tenantId, cancellationToken));
            recommendations.AddRange(await CheckAntiSpamPoliciesAsync(tenantId, cancellationToken));
            recommendations.AddRange(await CheckAntiMalwarePoliciesAsync(tenantId, cancellationToken));
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to analyze Exchange configuration for tenant {TenantId}", tenantId);
        }

        return recommendations;
    }

    public async Task<int> CalculateScoreAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        try {
            var recommendations = await AnalyzeAsync(tenantId, cancellationToken);
            var passedChecks = recommendations.Count(r => r.Status == "Passed");
            var totalChecks = recommendations.Count;

            return totalChecks > 0 ? (int)((double)passedChecks / totalChecks * MaxScore) : 0;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to calculate Exchange score for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private async Task<List<BpaRecommendationDto>> CheckTransportRulesAsync(Guid tenantId, CancellationToken cancellationToken) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var pagingParams = new PagingParameters { PageNumber = 1, PageSize = 100 };
            var rulesResponse = await _transportRuleService.GetTransportRulesAsync(tenantId, pagingParams, cancellationToken);
            var rules = rulesResponse.Items ?? new List<CIPP.Shared.DTOs.Exchange.TransportRuleDto>();

            var externalForwardingRules = rules.Where(r => 
                (r.Name ?? string.Empty).Contains("forward", StringComparison.OrdinalIgnoreCase) ||
                (r.Name ?? string.Empty).Contains("redirect", StringComparison.OrdinalIgnoreCase)
            ).ToList();

            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "External Mail Forwarding Rules",
                Description = externalForwardingRules.Any()
                    ? $"Found {externalForwardingRules.Count} potential external forwarding rule(s). Review for security risks."
                    : "No external forwarding rules detected in transport rules",
                Severity = externalForwardingRules.Any() ? "Medium" : "Low",
                Status = externalForwardingRules.Any() ? "Warning" : "Passed",
                RemediationSteps = "Review transport rules for external forwarding and ensure they align with security policies",
                StandardType = "Exchange",
                RelatedControl = "TransportRules"
            });

            var enabledRulesCount = rules.Count(r => r.State == "Enabled");
            var totalRulesCount = rules.Count;

            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Transport Rules Configuration",
                Description = $"Found {totalRulesCount} transport rule(s), {enabledRulesCount} enabled",
                Severity = "Low",
                Status = totalRulesCount > 0 ? "Passed" : "Warning",
                RemediationSteps = "Ensure transport rules are configured for mail flow security",
                StandardType = "Exchange",
                RelatedControl = "TransportRules"
            });
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to check transport rules for tenant {TenantId}", tenantId);
            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Transport Rules Analysis",
                Description = "Unable to analyze transport rules",
                Severity = "Low",
                Status = "Warning",
                StandardType = "Exchange"
            });
        }

        return recommendations;
    }

    private async Task<List<BpaRecommendationDto>> CheckAntiSpamPoliciesAsync(Guid tenantId, CancellationToken cancellationToken) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var pagingParams = new PagingParameters { PageNumber = 1, PageSize = 50 };
            var policiesResponse = await _spamFilterService.GetAntiSpamPoliciesAsync(tenantId, pagingParams, cancellationToken);
            var policies = policiesResponse.Items ?? new List<CIPP.Shared.DTOs.Exchange.HostedContentFilterPolicyDto>();

            var hasPolicies = policies.Any();

            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Anti-Spam Policies",
                Description = hasPolicies
                    ? $"Found {policies.Count} anti-spam policy/policies configured"
                    : "No anti-spam policies found. Default protection may not be sufficient",
                Severity = hasPolicies ? "Low" : "High",
                Status = hasPolicies ? "Passed" : "Failed",
                RemediationSteps = hasPolicies
                    ? "Review anti-spam policies for optimal protection settings"
                    : "Configure anti-spam policies to protect against spam and phishing",
                StandardType = "Exchange",
                RelatedControl = "AntiSpam"
            });

            if (hasPolicies) {
                var policiesWithHighSpamAction = policies.Count(p => 
                    (p.HighConfidenceSpamAction ?? string.Empty).Equals("Quarantine", StringComparison.OrdinalIgnoreCase) ||
                    (p.HighConfidenceSpamAction ?? string.Empty).Equals("Delete", StringComparison.OrdinalIgnoreCase)
                );

                var hasProperHighSpamHandling = policiesWithHighSpamAction > 0;

                recommendations.Add(new BpaRecommendationDto {
                    Category = Category,
                    Title = "High Confidence Spam Handling",
                    Description = hasProperHighSpamHandling
                        ? "High confidence spam is properly quarantined or deleted"
                        : "High confidence spam may not be properly handled. Consider quarantine or delete actions",
                    Severity = "Medium",
                    Status = hasProperHighSpamHandling ? "Passed" : "Warning",
                    RemediationSteps = "Configure high confidence spam action to Quarantine or Delete in anti-spam policies",
                    StandardType = "Exchange",
                    RelatedControl = "AntiSpam"
                });
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to check anti-spam policies for tenant {TenantId}", tenantId);
            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Anti-Spam Policy Analysis",
                Description = "Unable to analyze anti-spam policies",
                Severity = "Medium",
                Status = "Warning",
                StandardType = "Exchange"
            });
        }

        return recommendations;
    }

    private async Task<List<BpaRecommendationDto>> CheckAntiMalwarePoliciesAsync(Guid tenantId, CancellationToken cancellationToken) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var pagingParams = new PagingParameters { PageNumber = 1, PageSize = 50 };
            var policiesResponse = await _spamFilterService.GetAntiMalwarePoliciesAsync(tenantId, pagingParams, cancellationToken);
            var policies = policiesResponse.Items ?? new List<CIPP.Shared.DTOs.Exchange.MalwareFilterPolicyDto>();

            var hasPolicies = policies.Any();

            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Anti-Malware Policies",
                Description = hasPolicies
                    ? $"Found {policies.Count} anti-malware policy/policies configured"
                    : "No anti-malware policies found. Malware protection may be insufficient",
                Severity = hasPolicies ? "Low" : "High",
                Status = hasPolicies ? "Passed" : "Failed",
                RemediationSteps = hasPolicies
                    ? "Review anti-malware policies for comprehensive protection"
                    : "Configure anti-malware policies to protect against malicious attachments",
                StandardType = "Exchange",
                RelatedControl = "AntiMalware"
            });

            if (hasPolicies) {
                var policiesWithCommonAttachmentFiltering = policies.Count(p => p.EnableFileFilter == true);
                var hasFileFiltering = policiesWithCommonAttachmentFiltering > 0;

                recommendations.Add(new BpaRecommendationDto {
                    Category = Category,
                    Title = "Common Attachment Type Filtering",
                    Description = hasFileFiltering
                        ? "Common attachment type filtering is enabled"
                        : "Common attachment type filtering is not enabled. Enable to block risky file types",
                    Severity = "Medium",
                    Status = hasFileFiltering ? "Passed" : "Warning",
                    RemediationSteps = "Enable common attachment type filtering in anti-malware policies to block executable and script files",
                    StandardType = "Exchange",
                    RelatedControl = "AntiMalware"
                });
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to check anti-malware policies for tenant {TenantId}", tenantId);
            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Anti-Malware Policy Analysis",
                Description = "Unable to analyze anti-malware policies",
                Severity = "Medium",
                Status = "Warning",
                StandardType = "Exchange"
            });
        }

        return recommendations;
    }
}
