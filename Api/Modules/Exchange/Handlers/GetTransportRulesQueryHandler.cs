using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetTransportRulesQueryHandler : IRequestHandler<GetTransportRulesQuery, Task<List<TransportRuleDto>>> {
    private readonly ITransportRuleService _transportRuleService;

    public GetTransportRulesQueryHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task<List<TransportRuleDto>> Handle(GetTransportRulesQuery query, CancellationToken cancellationToken) {
        return await _transportRuleService.GetTransportRulesAsync(query.TenantId, cancellationToken);
    }
}
