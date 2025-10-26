using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecurityIncidentService {
    Task<SecurityIncidentsResponseDto> GetIncidentsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<SecurityIncidentDto?> GetIncidentAsync(Guid tenantId, string incidentId, CancellationToken cancellationToken = default);
    Task UpdateIncidentAsync(Guid tenantId, string incidentId, UpdateSecurityIncidentDto updateDto, CancellationToken cancellationToken = default);
}
