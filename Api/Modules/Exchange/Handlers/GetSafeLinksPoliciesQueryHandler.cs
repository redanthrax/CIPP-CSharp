using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetSafeLinksPoliciesQueryHandler : IRequestHandler<GetSafeLinksPoliciesQuery, Task<PagedResponse<SafeLinksPolicyDto>>> {
    private readonly ISafePoliciesService _safePoliciesService;

    public GetSafeLinksPoliciesQueryHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task<PagedResponse<SafeLinksPolicyDto>> Handle(GetSafeLinksPoliciesQuery query, CancellationToken cancellationToken) {
        return await _safePoliciesService.GetSafeLinksPoliciesAsync(query.TenantId, query.PagingParameters, cancellationToken);
    }
}
