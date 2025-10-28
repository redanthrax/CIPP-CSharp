using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Analyzers;

public class ConditionalAccessBpaAnalyzer : IBpaAnalyzer {
    private readonly IConditionalAccessPolicyService _policyService;
    private readonly ILogger<ConditionalAccessBpaAnalyzer> _logger;

    public string Category => "ConditionalAccess";
    public int MaxScore => 100;

    public ConditionalAccessBpaAnalyzer(IConditionalAccessPolicyService policyService, ILogger<ConditionalAccessBpaAnalyzer> logger) {
        _policyService = policyService;
        _logger = logger;
    }

    public async Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var pagingParams = new PagingParameters { PageNumber = 1, PageSize = 100 };
            var policiesResponse = await _policyService.GetPoliciesAsync(tenantId, pagingParams, cancellationToken);
            var policies = policiesResponse.Items ?? new List<CIPP.Shared.DTOs.ConditionalAccess.ConditionalAccessPolicyDto>();

            recommendations.AddRange(await CheckPolicyCoverageAsync(policies));
            recommendations.AddRange(await CheckMfaEnforcementAsync(policies));
            recommendations.AddRange(await CheckLegacyAuthBlockingAsync(policies));
            recommendations.AddRange(await CheckDeviceComplianceAsync(policies));
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to analyze Conditional Access configuration for tenant {TenantId}", tenantId);
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
            _logger.LogError(ex, "Failed to calculate Conditional Access score for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private Task<List<BpaRecommendationDto>> CheckPolicyCoverageAsync(List<CIPP.Shared.DTOs.ConditionalAccess.ConditionalAccessPolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var enabledPolicies = policies.Where(p => p.State == "enabled").ToList();
        var totalPolicies = policies.Count;

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Conditional Access Policy Coverage",
            Description = totalPolicies > 0
                ? $"Found {totalPolicies} policy/policies, {enabledPolicies.Count} enabled"
                : "No Conditional Access policies found. Create policies to protect your environment",
            Severity = totalPolicies > 0 ? "Low" : "High",
            Status = totalPolicies > 0 ? "Passed" : "Failed",
            RemediationSteps = totalPolicies > 0
                ? "Review policies to ensure comprehensive coverage of users, apps, and scenarios"
                : "Create Conditional Access policies to enforce MFA, block legacy auth, and require device compliance",
            StandardType = "ConditionalAccess",
            RelatedControl = "PolicyCoverage"
        });

        var disabledPolicies = policies.Where(p => p.State != "enabled").ToList();
        if (disabledPolicies.Any()) {
            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Disabled Conditional Access Policies",
                Description = $"Found {disabledPolicies.Count} disabled policy/policies. Review and enable or remove",
                Severity = "Low",
                Status = "Warning",
                RemediationSteps = "Review disabled policies and either enable them or delete if no longer needed",
                StandardType = "ConditionalAccess",
                RelatedControl = "PolicyState"
            });
        }

        return Task.FromResult(recommendations);
    }

    private Task<List<BpaRecommendationDto>> CheckMfaEnforcementAsync(List<CIPP.Shared.DTOs.ConditionalAccess.ConditionalAccessPolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var mfaPolicies = policies.Where(p =>
            p.State == "enabled" &&
            p.GrantControls != null &&
            (p.GrantControls.BuiltInControls?.Contains("mfa") == true ||
             p.GrantControls.BuiltInControls?.Contains("Mfa") == true)
        ).ToList();

        var hasMfaPolicies = mfaPolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Multi-Factor Authentication (MFA) Enforcement",
            Description = hasMfaPolicies
                ? $"Found {mfaPolicies.Count} enabled policy/policies requiring MFA"
                : "No policies found requiring MFA. Enable MFA for all users and administrators",
            Severity = hasMfaPolicies ? "Low" : "High",
            Status = hasMfaPolicies ? "Passed" : "Failed",
            RemediationSteps = hasMfaPolicies
                ? "Review MFA policies to ensure they cover all critical scenarios and users"
                : "Create Conditional Access policies requiring MFA for all users or at minimum for administrators",
            StandardType = "ConditionalAccess",
            RelatedControl = "MFA"
        });

        var adminMfaPolicies = policies.Where(p =>
            p.State == "enabled" &&
            p.GrantControls != null &&
            (p.GrantControls.BuiltInControls?.Contains("mfa") == true ||
             p.GrantControls.BuiltInControls?.Contains("Mfa") == true) &&
            p.Conditions?.Users?.IncludeRoles != null &&
            p.Conditions.Users.IncludeRoles.Any()
        ).ToList();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Administrator MFA Protection",
            Description = adminMfaPolicies.Any()
                ? $"Found {adminMfaPolicies.Count} policy/policies requiring MFA for administrative roles"
                : "No dedicated MFA policies for administrative roles. Administrators should always require MFA",
            Severity = adminMfaPolicies.Any() ? "Low" : "High",
            Status = adminMfaPolicies.Any() ? "Passed" : "Failed",
            RemediationSteps = "Create a Conditional Access policy requiring MFA for all administrative roles",
            StandardType = "ConditionalAccess",
            RelatedControl = "AdminMFA"
        });

        return Task.FromResult(recommendations);
    }

    private Task<List<BpaRecommendationDto>> CheckLegacyAuthBlockingAsync(List<CIPP.Shared.DTOs.ConditionalAccess.ConditionalAccessPolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var legacyAuthBlockingPolicies = policies.Where(p =>
            p.State == "enabled" &&
            p.Conditions?.ClientAppTypes != null &&
            (p.Conditions.ClientAppTypes.Contains("exchangeActiveSync") ||
             p.Conditions.ClientAppTypes.Contains("other")) &&
            p.GrantControls?.BuiltInControls?.Contains("block") == true
        ).ToList();

        var hasLegacyAuthBlocking = legacyAuthBlockingPolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Legacy Authentication Blocking",
            Description = hasLegacyAuthBlocking
                ? $"Found {legacyAuthBlockingPolicies.Count} policy/policies blocking legacy authentication"
                : "No policies found blocking legacy authentication. Block legacy auth to prevent security risks",
            Severity = hasLegacyAuthBlocking ? "Low" : "High",
            Status = hasLegacyAuthBlocking ? "Passed" : "Failed",
            RemediationSteps = hasLegacyAuthBlocking
                ? "Ensure legacy authentication blocking covers all scenarios"
                : "Create a Conditional Access policy to block legacy authentication protocols (Exchange ActiveSync, POP, IMAP)",
            StandardType = "ConditionalAccess",
            RelatedControl = "LegacyAuth"
        });

        return Task.FromResult(recommendations);
    }

    private Task<List<BpaRecommendationDto>> CheckDeviceComplianceAsync(List<CIPP.Shared.DTOs.ConditionalAccess.ConditionalAccessPolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var deviceCompliancePolicies = policies.Where(p =>
            p.State == "enabled" &&
            p.GrantControls != null &&
            (p.GrantControls.BuiltInControls?.Contains("compliantDevice") == true ||
             p.GrantControls.BuiltInControls?.Contains("domainJoinedDevice") == true)
        ).ToList();

        var hasDeviceCompliance = deviceCompliancePolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Device Compliance Requirements",
            Description = hasDeviceCompliance
                ? $"Found {deviceCompliancePolicies.Count} policy/policies requiring device compliance or domain join"
                : "No policies found requiring device compliance. Consider requiring compliant devices for sensitive apps",
            Severity = hasDeviceCompliance ? "Low" : "Medium",
            Status = hasDeviceCompliance ? "Passed" : "Warning",
            RemediationSteps = hasDeviceCompliance
                ? "Review device compliance policies to ensure they cover critical applications and scenarios"
                : "Create Conditional Access policies requiring compliant or domain-joined devices for sensitive applications",
            StandardType = "ConditionalAccess",
            RelatedControl = "DeviceCompliance"
        });

        return Task.FromResult(recommendations);
    }
}
