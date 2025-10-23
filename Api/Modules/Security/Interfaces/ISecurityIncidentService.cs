using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecurityIncidentService {
    Task<SecurityIncidentsResponseDto> GetIncidentsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<SecurityIncidentDto?> GetIncidentAsync(string tenantId, string incidentId, CancellationToken cancellationToken = default);
    Task UpdateIncidentAsync(string tenantId, string incidentId, UpdateSecurityIncidentDto updateDto, CancellationToken cancellationToken = default);
}
