using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.ActiveSync;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.ActiveSync;

public class GetActiveSyncDeviceAccessRulesQueryHandler : IRequestHandler<GetActiveSyncDeviceAccessRulesQuery, Task<PagedResponse<ActiveSyncDeviceAccessRuleDto>>> {
    private readonly IActiveSyncService _activeSyncService;

    public GetActiveSyncDeviceAccessRulesQueryHandler(IActiveSyncService activeSyncService) {
        _activeSyncService = activeSyncService;
    }

    public async Task<PagedResponse<ActiveSyncDeviceAccessRuleDto>> Handle(GetActiveSyncDeviceAccessRulesQuery query, CancellationToken cancellationToken) {
        return await _activeSyncService.GetDeviceAccessRulesAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}