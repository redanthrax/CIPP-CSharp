using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetTransportRulesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetTransportRulesQuery, Task<PagedResponse<TransportRuleDto>>>;
