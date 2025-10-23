using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecurityIncidentsQueryHandler : IRequestHandler<GetSecurityIncidentsQuery, Task<SecurityIncidentsResponseDto>> {
    private readonly ISecurityIncidentService _incidentService;

    public GetSecurityIncidentsQueryHandler(ISecurityIncidentService incidentService) {
        _incidentService = incidentService;
    }

    public async Task<SecurityIncidentsResponseDto> Handle(GetSecurityIncidentsQuery query, CancellationToken cancellationToken) {
        return await _incidentService.GetIncidentsAsync(query.TenantId, cancellationToken);
    }
}
