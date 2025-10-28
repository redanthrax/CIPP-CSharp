using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Analyzers;

public class IntuneBpaAnalyzer : IBpaAnalyzer {
    private readonly IIntunePolicyService _policyService;
    private readonly ILogger<IntuneBpaAnalyzer> _logger;

    public string Category => "Intune";
    public int MaxScore => 100;

    public IntuneBpaAnalyzer(IIntunePolicyService policyService, ILogger<IntuneBpaAnalyzer> logger) {
        _policyService = policyService;
        _logger = logger;
    }

    public async Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var policies = await _policyService.GetPoliciesAsync(tenantId, cancellationToken);

            recommendations.AddRange(await CheckCompliancePoliciesAsync(policies));
            recommendations.AddRange(await CheckConfigurationPoliciesAsync(policies));
            recommendations.AddRange(await CheckSecurityBaselinesAsync(policies));
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to analyze Intune configuration for tenant {TenantId}", tenantId);
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
            _logger.LogError(ex, "Failed to calculate Intune score for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private Task<List<BpaRecommendationDto>> CheckCompliancePoliciesAsync(List<CIPP.Shared.DTOs.Intune.IntunePolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var compliancePolicies = policies.Where(p =>
            (p.PolicyTypeName ?? string.Empty).Contains("Compliance", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        var hasCompliancePolicies = compliancePolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Device Compliance Policies",
            Description = hasCompliancePolicies
                ? $"Found {compliancePolicies.Count} device compliance policy/policies"
                : "No device compliance policies found. Create policies to enforce device security requirements",
            Severity = hasCompliancePolicies ? "Low" : "High",
            Status = hasCompliancePolicies ? "Passed" : "Failed",
            RemediationSteps = hasCompliancePolicies
                ? "Review compliance policies to ensure they cover all platforms and enforce appropriate security baselines"
                : "Create device compliance policies for Windows, iOS, Android, and macOS devices",
            StandardType = "Intune",
            RelatedControl = "CompliancePolicies"
        });

        if (hasCompliancePolicies) {
            var windowsCompliancePolicies = compliancePolicies.Where(p =>
                (p.DisplayName ?? string.Empty).Contains("Windows", StringComparison.OrdinalIgnoreCase) ||
                (p.PolicyTypeName ?? string.Empty).Contains("Windows", StringComparison.OrdinalIgnoreCase)
            ).ToList();

            recommendations.Add(new BpaRecommendationDto {
                Category = Category,
                Title = "Windows Compliance Coverage",
                Description = windowsCompliancePolicies.Any()
                    ? $"Found {windowsCompliancePolicies.Count} Windows compliance policy/policies"
                    : "No Windows-specific compliance policies found. Create policies for Windows devices",
                Severity = windowsCompliancePolicies.Any() ? "Low" : "Medium",
                Status = windowsCompliancePolicies.Any() ? "Passed" : "Warning",
                RemediationSteps = "Create Windows compliance policies enforcing BitLocker, antivirus, firewall, and OS version requirements",
                StandardType = "Intune",
                RelatedControl = "WindowsCompliance"
            });
        }

        return Task.FromResult(recommendations);
    }

    private Task<List<BpaRecommendationDto>> CheckConfigurationPoliciesAsync(List<CIPP.Shared.DTOs.Intune.IntunePolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var configurationPolicies = policies.Where(p =>
            (p.PolicyTypeName ?? string.Empty).Contains("Configuration", StringComparison.OrdinalIgnoreCase) ||
            (p.PolicyTypeName ?? string.Empty).Contains("DeviceConfig", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        var hasConfigurationPolicies = configurationPolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Device Configuration Policies",
            Description = hasConfigurationPolicies
                ? $"Found {configurationPolicies.Count} device configuration policy/policies"
                : "No device configuration policies found. Create policies to manage device settings",
            Severity = hasConfigurationPolicies ? "Low" : "Medium",
            Status = hasConfigurationPolicies ? "Passed" : "Warning",
            RemediationSteps = hasConfigurationPolicies
                ? "Review configuration policies to ensure comprehensive device management"
                : "Create device configuration policies for security settings, Wi-Fi, VPN, and certificates",
            StandardType = "Intune",
            RelatedControl = "ConfigurationPolicies"
        });

        var bitlockerPolicies = policies.Where(p =>
            (p.DisplayName ?? string.Empty).Contains("BitLocker", StringComparison.OrdinalIgnoreCase) ||
            (p.DisplayName ?? string.Empty).Contains("Encryption", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "BitLocker/Encryption Configuration",
            Description = bitlockerPolicies.Any()
                ? $"Found {bitlockerPolicies.Count} encryption-related policy/policies"
                : "No BitLocker or encryption policies found. Enable disk encryption for data protection",
            Severity = bitlockerPolicies.Any() ? "Low" : "High",
            Status = bitlockerPolicies.Any() ? "Passed" : "Failed",
            RemediationSteps = bitlockerPolicies.Any()
                ? "Review encryption policies to ensure all devices are protected"
                : "Create policies to enforce BitLocker on Windows and encryption on mobile devices",
            StandardType = "Intune",
            RelatedControl = "Encryption"
        });

        var antivirusPolicies = policies.Where(p =>
            (p.DisplayName ?? string.Empty).Contains("Antivirus", StringComparison.OrdinalIgnoreCase) ||
            (p.DisplayName ?? string.Empty).Contains("Defender", StringComparison.OrdinalIgnoreCase) ||
            (p.DisplayName ?? string.Empty).Contains("EDR", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Antivirus/Defender Configuration",
            Description = antivirusPolicies.Any()
                ? $"Found {antivirusPolicies.Count} antivirus/Defender policy/policies"
                : "No antivirus or Defender policies found. Configure endpoint protection",
            Severity = antivirusPolicies.Any() ? "Low" : "High",
            Status = antivirusPolicies.Any() ? "Passed" : "Failed",
            RemediationSteps = antivirusPolicies.Any()
                ? "Review antivirus policies to ensure real-time protection and scanning are enabled"
                : "Create policies to configure Microsoft Defender Antivirus with real-time protection enabled",
            StandardType = "Intune",
            RelatedControl = "Antivirus"
        });

        return Task.FromResult(recommendations);
    }

    private Task<List<BpaRecommendationDto>> CheckSecurityBaselinesAsync(List<CIPP.Shared.DTOs.Intune.IntunePolicyDto> policies) {
        var recommendations = new List<BpaRecommendationDto>();

        var securityBaselinePolicies = policies.Where(p =>
            (p.PolicyTypeName ?? string.Empty).Contains("SecurityBaseline", StringComparison.OrdinalIgnoreCase) ||
            (p.DisplayName ?? string.Empty).Contains("Baseline", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        var hasSecurityBaselines = securityBaselinePolicies.Any();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "Security Baseline Policies",
            Description = hasSecurityBaselines
                ? $"Found {securityBaselinePolicies.Count} security baseline policy/policies"
                : "No security baseline policies found. Deploy baselines for standardized security configurations",
            Severity = hasSecurityBaselines ? "Low" : "Medium",
            Status = hasSecurityBaselines ? "Passed" : "Warning",
            RemediationSteps = hasSecurityBaselines
                ? "Review security baselines to ensure they are up to date and properly deployed"
                : "Deploy Microsoft security baselines for Windows 10/11, Edge, and Microsoft Defender",
            StandardType = "Intune",
            RelatedControl = "SecurityBaselines"
        });

        var appProtectionPolicies = policies.Where(p =>
            (p.PolicyTypeName ?? string.Empty).Contains("AppProtection", StringComparison.OrdinalIgnoreCase) ||
            (p.PolicyTypeName ?? string.Empty).Contains("MAM", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        recommendations.Add(new BpaRecommendationDto {
            Category = Category,
            Title = "App Protection Policies (MAM)",
            Description = appProtectionPolicies.Any()
                ? $"Found {appProtectionPolicies.Count} app protection policy/policies"
                : "No app protection policies found. Protect corporate data in mobile apps",
            Severity = appProtectionPolicies.Any() ? "Low" : "Medium",
            Status = appProtectionPolicies.Any() ? "Passed" : "Warning",
            RemediationSteps = appProtectionPolicies.Any()
                ? "Review app protection policies to ensure data protection for iOS and Android"
                : "Create app protection policies to prevent data leakage from managed apps on mobile devices",
            StandardType = "Intune",
            RelatedControl = "AppProtection"
        });

        return Task.FromResult(recommendations);
    }
}
