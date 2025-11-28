using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta.Models;
using System.Security.Cryptography;

namespace CIPP.Api.Modules.Alerts.Services;

public class BecRemediationAlertActionHandler : IAlertActionHandler {
    private readonly IMicrosoftGraphService _graphService;
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<BecRemediationAlertActionHandler> _logger;

    public string ActionType => "becremediate";

    public BecRemediationAlertActionHandler(
        IMicrosoftGraphService graphService,
        IExchangeOnlineService exoService,
        ILogger<BecRemediationAlertActionHandler> logger) {
        _graphService = graphService;
        _exoService = exoService;
        _logger = logger;
    }

    public async Task ExecuteAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string? alertComment = null,
        Dictionary<string, string>? additionalParams = null,
        CancellationToken cancellationToken = default) {
        try {
            var userId = enrichedData.TryGetValue("UserId", out var userIdObj) 
                ? userIdObj.ToString() 
                : enrichedData.TryGetValue("CIPPUserId", out var cippUserIdObj)
                    ? cippUserIdObj.ToString()
                    : null;

            if (string.IsNullOrEmpty(userId)) {
                _logger.LogWarning("No user ID found in enriched data, cannot perform BEC remediation");
                return;
            }

            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));

            _logger.LogInformation("Starting BEC remediation for user {UserId} in tenant {TenantId}", userId, tenantId);

            await DisableUserAccountAsync(graphClient, userId, cancellationToken);
            var newPassword = await ResetUserPasswordAsync(graphClient, userId, cancellationToken);
            await RevokeUserSessionsAsync(graphClient, userId, cancellationToken);
            await DisableInboxRulesAsync(tenantId, userId, cancellationToken);

            _logger.LogInformation(
                "BEC remediation completed successfully for user {UserId}. New password: {Password}",
                userId, newPassword);
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Failed to perform BEC remediation for tenant {TenantId}",
                tenantId);
            throw;
        }
    }

    private async Task DisableUserAccountAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient,
        string userId,
        CancellationToken cancellationToken) {
        try {
            var user = new User {
                AccountEnabled = false
            };

            await graphClient.Users[userId].PatchAsync(user, cancellationToken: cancellationToken);
            _logger.LogInformation("Disabled user account {UserId}", userId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to disable user account {UserId}", userId);
            throw;
        }
    }

    private async Task<string> ResetUserPasswordAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient,
        string userId,
        CancellationToken cancellationToken) {
        try {
            var newPassword = GenerateSecurePassword();
            var user = new User {
                PasswordProfile = new PasswordProfile {
                    Password = newPassword,
                    ForceChangePasswordNextSignIn = true
                }
            };

            await graphClient.Users[userId].PatchAsync(user, cancellationToken: cancellationToken);
            _logger.LogInformation("Reset password for user {UserId}", userId);
            return newPassword;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to reset password for user {UserId}", userId);
            throw;
        }
    }

    private async Task RevokeUserSessionsAsync(
        Microsoft.Graph.Beta.GraphServiceClient graphClient,
        string userId,
        CancellationToken cancellationToken) {
        try {
            await graphClient.Users[userId].RevokeSignInSessions.PostAsRevokeSignInSessionsPostResponseAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Revoked all sessions for user {UserId}", userId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to revoke sessions for user {UserId}", userId);
            throw;
        }
    }

    private async Task DisableInboxRulesAsync(
        string tenantId,
        string userId,
        CancellationToken cancellationToken) {
        try {
            var getRulesParams = new Dictionary<string, object> {
                { "Mailbox", userId }
            };

            var rules = await _exoService.ExecuteCmdletListAsync<dynamic>(
                Guid.Parse(tenantId),
                "Get-InboxRule",
                getRulesParams,
                cancellationToken);

            if (rules == null || !rules.Any()) {
                _logger.LogInformation("No inbox rules found for user {UserId}", userId);
                return;
            }

            foreach (var rule in rules) {
                try {
                    string? ruleIdentity = null;
                    try {
                        ruleIdentity = rule.Identity?.ToString() ?? rule.Name?.ToString();
                    } catch {
                        ruleIdentity = null;
                    }
                    
                    if (!string.IsNullOrEmpty(ruleIdentity)) {
                        var disableParams = new Dictionary<string, object> {
                            { "Mailbox", userId },
                            { "Identity", ruleIdentity },
                            { "Enabled", false }
                        };

                        await _exoService.ExecuteCmdletNoResultAsync(
                            Guid.Parse(tenantId),
                            "Set-InboxRule",
                            disableParams,
                            cancellationToken);

                        var ruleIdStr = ruleIdentity;
                        _logger.LogInformation("Disabled inbox rule {RuleId} for user {UserId}", ruleIdStr, userId);
                    }
                } catch (Exception ex) {
                    _logger.LogWarning(ex, "Failed to disable individual inbox rule for user {UserId}", userId);
                }
            }

            _logger.LogInformation("Disabled {Count} inbox rules for user {UserId}", rules.Count, userId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to disable inbox rules for user {UserId}", userId);
        }
    }

    private static string GenerateSecurePassword() {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string special = "!@#$%^&*";
        const int passwordLength = 20;
        
        var allChars = upperCase + lowerCase + digits + special;
        var password = new char[passwordLength];
        
        password[0] = upperCase[RandomNumberGenerator.GetInt32(upperCase.Length)];
        password[1] = lowerCase[RandomNumberGenerator.GetInt32(lowerCase.Length)];
        password[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        password[3] = special[RandomNumberGenerator.GetInt32(special.Length)];
        
        for (int i = 4; i < passwordLength; i++) {
            password[i] = allChars[RandomNumberGenerator.GetInt32(allChars.Length)];
        }
        
        return new string(password.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
    }
}
