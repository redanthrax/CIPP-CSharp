using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Shared.DTOs.Security;
using Microsoft.Graph.Beta.Models.Security;

namespace CIPP.Api.Modules.Security.Services;

public class SecurityIncidentService : ISecurityIncidentService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<SecurityIncidentService> _logger;

    public SecurityIncidentService(IMicrosoftGraphService graphService, ILogger<SecurityIncidentService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<SecurityIncidentsResponseDto> GetIncidentsAsync(string tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting security incidents for tenant {TenantId}", tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var incidents = await graphClient.Security.Incidents.GetAsync(cancellationToken: cancellationToken);
        
        if (incidents?.Value == null) {
            return new SecurityIncidentsResponseDto {
                Incidents = new List<SecurityIncidentDto>()
            };
        }

        var incidentDtos = incidents.Value.Select(i => MapToIncidentDto(i, tenantId)).ToList();
        
        return new SecurityIncidentsResponseDto {
            Incidents = incidentDtos,
            NewIncidentsCount = incidentDtos.Count(i => i.Status.Equals("new", StringComparison.OrdinalIgnoreCase)),
            InProgressIncidentsCount = incidentDtos.Count(i => i.Status.Equals("inProgress", StringComparison.OrdinalIgnoreCase)),
            HighSeverityCount = incidentDtos.Count(i => i.Severity.Equals("high", StringComparison.OrdinalIgnoreCase)),
            MediumSeverityCount = incidentDtos.Count(i => i.Severity.Equals("medium", StringComparison.OrdinalIgnoreCase)),
            LowSeverityCount = incidentDtos.Count(i => i.Severity.Equals("low", StringComparison.OrdinalIgnoreCase))
        };
    }

    public async Task<SecurityIncidentDto?> GetIncidentAsync(string tenantId, string incidentId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting security incident {IncidentId} for tenant {TenantId}", incidentId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var incident = await graphClient.Security.Incidents[incidentId].GetAsync(cancellationToken: cancellationToken);
        
        return incident != null ? MapToIncidentDto(incident, tenantId) : null;
    }

    public async Task UpdateIncidentAsync(string tenantId, string incidentId, UpdateSecurityIncidentDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating security incident {IncidentId} for tenant {TenantId}", incidentId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var incident = new Incident();
        
        if (!string.IsNullOrEmpty(updateDto.Status)) {
            incident.Status = Enum.TryParse<IncidentStatus>(updateDto.Status, true, out var status) ? status : null;
        }
        
        if (!string.IsNullOrEmpty(updateDto.Classification)) {
            incident.Classification = Enum.TryParse<AlertClassification>(updateDto.Classification, true, out var classification) ? classification : null;
        }
        
        if (!string.IsNullOrEmpty(updateDto.Determination)) {
            incident.Determination = Enum.TryParse<AlertDetermination>(updateDto.Determination, true, out var determination) ? determination : null;
        }
        
        if (!string.IsNullOrEmpty(updateDto.AssignedTo)) {
            incident.AssignedTo = updateDto.AssignedTo;
        }
        
        await graphClient.Security.Incidents[incidentId].PatchAsync(incident, cancellationToken: cancellationToken);
    }

    private static SecurityIncidentDto MapToIncidentDto(Incident incident, string tenantId) {
        return new SecurityIncidentDto {
            Id = incident.Id ?? string.Empty,
            DisplayName = incident.DisplayName ?? string.Empty,
            Status = incident.Status?.ToString() ?? string.Empty,
            Severity = incident.Severity?.ToString() ?? string.Empty,
            Classification = incident.Classification?.ToString(),
            Determination = incident.Determination?.ToString(),
            IncidentUrl = incident.IncidentWebUrl ?? string.Empty,
            RedirectId = incident.RedirectIncidentId,
            AssignedTo = incident.AssignedTo,
            CreatedDateTime = incident.CreatedDateTime?.DateTime,
            LastUpdateDateTime = incident.LastUpdateDateTime?.DateTime,
            Tags = new List<string>(),
            TenantId = tenantId
        };
    }
}
