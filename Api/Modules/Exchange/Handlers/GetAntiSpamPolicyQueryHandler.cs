using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetAntiSpamPolicyQueryHandler : IRequestHandler<GetAntiSpamPolicyQuery, Task<HostedContentFilterPolicyDto?>> {
    private readonly ISpamFilterService _spamFilterService;

    public GetAntiSpamPolicyQueryHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task<HostedContentFilterPolicyDto?> Handle(GetAntiSpamPolicyQuery query, CancellationToken cancellationToken) {
        return await _spamFilterService.GetAntiSpamPolicyAsync(query.TenantId, query.PolicyId, cancellationToken);
    }
}
