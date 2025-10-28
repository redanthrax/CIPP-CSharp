using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Analyzers;

public class IdentityBpaAnalyzer : IBpaAnalyzer {
    private readonly IAuthenticationMethodPolicyService _authMethodService;
    private readonly ILogger<IdentityBpaAnalyzer> _logger;

    public string Category => "Identity";
    public int MaxScore => 100;

    public IdentityBpaAnalyzer(IAuthenticationMethodPolicyService authMethodService, ILogger<IdentityBpaAnalyzer> logger) {
        _authMethodService = authMethodService;
        _logger = logger;
    }

    public async Task<List<BpaRecommendationDto>> AnalyzeAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var recommendations = new List<BpaRecommendationDto>();

        try {
            var policy = await _authMethodService.GetPolicyAsync(tenantId, cancellationToken);

            if (policy == null) {
                recommendations.Add(new BpaRecommendationDto {
                    Category = Category,
                    Title = "Authentication Method Policy Not Found",
                    Description = "Unable to retrieve authentication method policy for analysis",
                    Severity = "High",
                    Status = "Failed",
                    RemediationSteps = "Verify tenant access and permissions",
                    StandardType = "Identity"
                });
                return recommendations;
            }

            recommendations.Add(await CheckFido2Async(tenantId, cancellationToken));
            recommendations.Add(await CheckMicrosoftAuthenticatorAsync(tenantId, cancellationToken));
            recommendations.Add(await CheckTemporaryAccessPassAsync(tenantId, cancellationToken));
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to analyze identity controls for tenant {TenantId}", tenantId);
        }

        return recommendations.Where(r => r != null!).ToList();
    }

    public async Task<int> CalculateScoreAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        try {
            var recommendations = await AnalyzeAsync(tenantId, cancellationToken);
            var passedChecks = recommendations.Count(r => r.Status == "Passed");
            var totalChecks = recommendations.Count;

            return totalChecks > 0 ? (int)((double)passedChecks / totalChecks * MaxScore) : 0;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to calculate identity score for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private async Task<BpaRecommendationDto> CheckFido2Async(Guid tenantId, CancellationToken cancellationToken) {
        try {
            var fido2Config = await _authMethodService.GetMethodConfigAsync(tenantId, "fido2", cancellationToken);
            var isEnabled = fido2Config?.State == "enabled";

            return new BpaRecommendationDto {
                Category = Category,
                Title = "FIDO2 Security Keys",
                Description = isEnabled
                    ? "FIDO2 security keys are enabled for phishing-resistant authentication"
                    : "FIDO2 security keys are not enabled. Consider enabling for enhanced security",
                Severity = "High",
                Status = isEnabled ? "Passed" : "Failed",
                RemediationSteps = "Enable FIDO2 security keys in Authentication Methods policy",
                StandardType = "Identity",
                RelatedControl = "fido2"
            };
        } catch {
            return new BpaRecommendationDto {
                Category = Category,
                Title = "FIDO2 Security Keys",
                Description = "Unable to check FIDO2 configuration",
                Severity = "Medium",
                Status = "Warning",
                StandardType = "Identity"
            };
        }
    }

    private async Task<BpaRecommendationDto> CheckMicrosoftAuthenticatorAsync(Guid tenantId, CancellationToken cancellationToken) {
        try {
            var authConfig = await _authMethodService.GetMethodConfigAsync(tenantId, "microsoftAuthenticator", cancellationToken);
            var isEnabled = authConfig?.State == "enabled";

            return new BpaRecommendationDto {
                Category = Category,
                Title = "Microsoft Authenticator",
                Description = isEnabled
                    ? "Microsoft Authenticator is enabled for MFA"
                    : "Microsoft Authenticator is not enabled. Enable for improved user experience",
                Severity = "Medium",
                Status = isEnabled ? "Passed" : "Failed",
                RemediationSteps = "Enable Microsoft Authenticator in Authentication Methods policy",
                StandardType = "Identity",
                RelatedControl = "microsoftAuthenticator"
            };
        } catch {
            return new BpaRecommendationDto {
                Category = Category,
                Title = "Microsoft Authenticator",
                Description = "Unable to check Microsoft Authenticator configuration",
                Severity = "Low",
                Status = "Warning",
                StandardType = "Identity"
            };
        }
    }

    private async Task<BpaRecommendationDto> CheckTemporaryAccessPassAsync(Guid tenantId, CancellationToken cancellationToken) {
        try {
            var tapConfig = await _authMethodService.GetMethodConfigAsync(tenantId, "temporaryAccessPass", cancellationToken);
            var isEnabled = tapConfig?.State == "enabled";

            return new BpaRecommendationDto {
                Category = Category,
                Title = "Temporary Access Pass",
                Description = isEnabled
                    ? "Temporary Access Pass is enabled for passwordless onboarding"
                    : "Temporary Access Pass is not enabled. Consider enabling for easier passwordless adoption",
                Severity = "Low",
                Status = isEnabled ? "Passed" : "Warning",
                RemediationSteps = "Enable Temporary Access Pass for temporary passwordless access",
                StandardType = "Identity",
                RelatedControl = "temporaryAccessPass"
            };
        } catch {
            return new BpaRecommendationDto {
                Category = Category,
                Title = "Temporary Access Pass",
                Description = "Unable to check Temporary Access Pass configuration",
                Severity = "Low",
                Status = "Warning",
                StandardType = "Identity"
            };
        }
    }
}
