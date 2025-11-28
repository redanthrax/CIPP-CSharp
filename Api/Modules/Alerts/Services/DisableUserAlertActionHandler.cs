using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Alerts.Services;

public class DisableUserAlertActionHandler : IAlertActionHandler {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<DisableUserAlertActionHandler> _logger;

    public string ActionType => "disableuser";

    public DisableUserAlertActionHandler(
        IMicrosoftGraphService graphService,
        ILogger<DisableUserAlertActionHandler> logger) {
        _graphService = graphService;
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
                _logger.LogWarning("No user ID found in enriched data, cannot disable user");
                return;
            }

            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));

            var user = new User {
                AccountEnabled = false
            };

            await graphClient.Users[userId].PatchAsync(user, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Successfully disabled user {UserId} for tenant {TenantId}",
                userId, tenantId);
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Failed to disable user for tenant {TenantId}",
                tenantId);
            throw;
        }
    }
}
