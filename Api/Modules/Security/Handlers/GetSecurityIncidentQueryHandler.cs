using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecurityIncidentQueryHandler : IRequestHandler<GetSecurityIncidentQuery, Task<SecurityIncidentDto?>> {
    private readonly ISecurityIncidentService _incidentService;

    public GetSecurityIncidentQueryHandler(ISecurityIncidentService incidentService) {
        _incidentService = incidentService;
    }

    public async Task<SecurityIncidentDto?> Handle(GetSecurityIncidentQuery query, CancellationToken cancellationToken) {
        return await _incidentService.GetIncidentAsync(query.TenantId, query.IncidentId, cancellationToken);
    }
}
