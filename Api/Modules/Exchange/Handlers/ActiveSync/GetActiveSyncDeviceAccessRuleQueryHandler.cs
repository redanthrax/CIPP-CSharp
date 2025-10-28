using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.ActiveSync;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.ActiveSync;

public class GetActiveSyncDeviceAccessRuleQueryHandler : IRequestHandler<GetActiveSyncDeviceAccessRuleQuery, Task<ActiveSyncDeviceAccessRuleDto?>> {
    private readonly IActiveSyncService _activeSyncService;

    public GetActiveSyncDeviceAccessRuleQueryHandler(IActiveSyncService activeSyncService) {
        _activeSyncService = activeSyncService;
    }

    public async Task<ActiveSyncDeviceAccessRuleDto?> Handle(GetActiveSyncDeviceAccessRuleQuery query, CancellationToken cancellationToken) {
        return await _activeSyncService.GetDeviceAccessRuleAsync(query.TenantId, query.RuleIdentity, cancellationToken);
    }
}