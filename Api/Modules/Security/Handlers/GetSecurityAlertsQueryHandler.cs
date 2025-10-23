using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecurityAlertsQueryHandler : IRequestHandler<GetSecurityAlertsQuery, Task<SecurityAlertsResponseDto>> {
    private readonly ISecurityAlertService _alertService;

    public GetSecurityAlertsQueryHandler(ISecurityAlertService alertService) {
        _alertService = alertService;
    }

    public async Task<SecurityAlertsResponseDto> Handle(GetSecurityAlertsQuery query, CancellationToken cancellationToken) {
        return await _alertService.GetAlertsAsync(query.TenantId, query.ServiceSource, cancellationToken);
    }
}
