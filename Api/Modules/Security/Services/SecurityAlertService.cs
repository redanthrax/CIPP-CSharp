using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Shared.DTOs.Security;
using Microsoft.Graph.Beta.Models.Security;

namespace CIPP.Api.Modules.Security.Services;

public class SecurityAlertService : ISecurityAlertService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<SecurityAlertService> _logger;

    public SecurityAlertService(IMicrosoftGraphService graphService, ILogger<SecurityAlertService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<SecurityAlertsResponseDto> GetAlertsAsync(Guid tenantId, string? serviceSource = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting security alerts for tenant {TenantId} with service source filter: {ServiceSource}", tenantId, serviceSource ?? "none");
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var alerts = await graphClient.Security.Alerts_v2.GetAsync(requestConfiguration => {
            if (!string.IsNullOrEmpty(serviceSource)) {
                requestConfiguration.QueryParameters.Filter = $"serviceSource eq '{serviceSource}'";
            }
        }, cancellationToken: cancellationToken);
        
        if (alerts?.Value == null) {
            return new SecurityAlertsResponseDto {
                Alerts = new List<SecurityAlertDto>()
            };
        }

        var alertDtos = alerts.Value.Select(a => MapToAlertDto(a, tenantId)).ToList();
        var activeAlerts = alertDtos.Where(a => 
            a.Status.Equals("newAlert", StringComparison.OrdinalIgnoreCase) || 
            a.Status.Equals("inProgress", StringComparison.OrdinalIgnoreCase)
        ).ToList();
        
        return new SecurityAlertsResponseDto {
            Alerts = alertDtos,
            NewAlertsCount = alertDtos.Count(a => a.Status.Equals("newAlert", StringComparison.OrdinalIgnoreCase)),
            InProgressAlertsCount = alertDtos.Count(a => a.Status.Equals("inProgress", StringComparison.OrdinalIgnoreCase)),
            SeverityHighAlertsCount = activeAlerts.Count(a => a.Severity.Equals("high", StringComparison.OrdinalIgnoreCase)),
            SeverityMediumAlertsCount = activeAlerts.Count(a => a.Severity.Equals("medium", StringComparison.OrdinalIgnoreCase)),
            SeverityLowAlertsCount = activeAlerts.Count(a => a.Severity.Equals("low", StringComparison.OrdinalIgnoreCase)),
            SeverityInformationalCount = activeAlerts.Count(a => a.Severity.Equals("informational", StringComparison.OrdinalIgnoreCase))
        };
    }

    public async Task<SecurityAlertDto?> GetAlertAsync(Guid tenantId, string alertId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting security alert {AlertId} for tenant {TenantId}", alertId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var alert = await graphClient.Security.Alerts_v2[alertId].GetAsync(cancellationToken: cancellationToken);
        
        return alert != null ? MapToAlertDto(alert, tenantId) : null;
    }

    public async Task UpdateAlertAsync(Guid tenantId, string alertId, UpdateSecurityAlertDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating security alert {AlertId} for tenant {TenantId}", alertId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var alert = new Alert();
        
        if (!string.IsNullOrEmpty(updateDto.Status)) {
            alert.Status = Enum.TryParse<AlertStatus>(updateDto.Status, true, out var status) ? status : null;
        }
        
        if (!string.IsNullOrEmpty(updateDto.AssignedTo)) {
            alert.AssignedTo = updateDto.AssignedTo;
        }
        
        await graphClient.Security.Alerts_v2[alertId].PatchAsync(alert, cancellationToken: cancellationToken);
    }

    private static SecurityAlertDto MapToAlertDto(Alert alert, Guid tenantId) {
        return new SecurityAlertDto {
            Id = alert.Id ?? string.Empty,
            Title = alert.Title ?? string.Empty,
            Category = alert.Category ?? string.Empty,
            Severity = alert.Severity?.ToString() ?? string.Empty,
            Status = alert.Status?.ToString() ?? string.Empty,
            EventDateTime = alert.CreatedDateTime?.DateTime,
            InvolvedUsers = new List<string>(),
            TenantId = tenantId
        };
    }
}
