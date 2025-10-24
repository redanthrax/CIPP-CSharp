using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetAntiSpamPoliciesQueryHandler : IRequestHandler<GetAntiSpamPoliciesQuery, Task<PagedResponse<HostedContentFilterPolicyDto>>> {
    private readonly ISpamFilterService _spamFilterService;

    public GetAntiSpamPoliciesQueryHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task<PagedResponse<HostedContentFilterPolicyDto>> Handle(GetAntiSpamPoliciesQuery query, CancellationToken cancellationToken) {
        return await _spamFilterService.GetAntiSpamPoliciesAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}
