using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.ActiveSync;

public record GetActiveSyncDeviceAccessRuleQuery(Guid TenantId, string RuleIdentity)
    : IRequest<GetActiveSyncDeviceAccessRuleQuery, Task<ActiveSyncDeviceAccessRuleDto?>>;