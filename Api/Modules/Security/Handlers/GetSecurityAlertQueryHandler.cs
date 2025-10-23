using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecurityAlertQueryHandler : IRequestHandler<GetSecurityAlertQuery, Task<SecurityAlertDto?>> {
    private readonly ISecurityAlertService _alertService;

    public GetSecurityAlertQueryHandler(ISecurityAlertService alertService) {
        _alertService = alertService;
    }

    public async Task<SecurityAlertDto?> Handle(GetSecurityAlertQuery query, CancellationToken cancellationToken) {
        return await _alertService.GetAlertAsync(query.TenantId, query.AlertId, cancellationToken);
    }
}
