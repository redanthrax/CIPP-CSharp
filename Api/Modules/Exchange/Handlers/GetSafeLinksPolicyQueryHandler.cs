using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetSafeLinksPolicyQueryHandler : IRequestHandler<GetSafeLinksPolicyQuery, Task<SafeLinksPolicyDto?>> {
    private readonly ISafePoliciesService _safePoliciesService;

    public GetSafeLinksPolicyQueryHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task<SafeLinksPolicyDto?> Handle(GetSafeLinksPolicyQuery query, CancellationToken cancellationToken) {
        return await _safePoliciesService.GetSafeLinksPolicyAsync(query.TenantId, query.PolicyName, cancellationToken);
    }
}
