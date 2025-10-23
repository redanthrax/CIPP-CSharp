using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetTransportRulesQuery(string TenantId) : IRequest<GetTransportRulesQuery, Task<List<TransportRuleDto>>>;
