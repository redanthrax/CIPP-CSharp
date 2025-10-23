using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetTransportRuleQuery(string TenantId, string RuleId) : IRequest<GetTransportRuleQuery, Task<TransportRuleDetailsDto?>>;
