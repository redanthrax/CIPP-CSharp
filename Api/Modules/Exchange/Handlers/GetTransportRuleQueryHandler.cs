using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetTransportRuleQueryHandler : IRequestHandler<GetTransportRuleQuery, Task<TransportRuleDetailsDto?>> {
    private readonly ITransportRuleService _transportRuleService;

    public GetTransportRuleQueryHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task<TransportRuleDetailsDto?> Handle(GetTransportRuleQuery query, CancellationToken cancellationToken) {
        return await _transportRuleService.GetTransportRuleAsync(query.TenantId, query.RuleId, cancellationToken);
    }
}
