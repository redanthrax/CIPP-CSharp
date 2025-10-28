using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.ActiveSync;

public record GetActiveSyncDeviceAccessRulesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetActiveSyncDeviceAccessRulesQuery, Task<PagedResponse<ActiveSyncDeviceAccessRuleDto>>>;