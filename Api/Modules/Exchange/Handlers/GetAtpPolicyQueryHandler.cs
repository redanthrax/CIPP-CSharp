using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetAtpPolicyQueryHandler : IRequestHandler<GetAtpPolicyQuery, Task<AtpPolicyDto?>> {
    private readonly ISafePoliciesService _safePoliciesService;

    public GetAtpPolicyQueryHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task<AtpPolicyDto?> Handle(GetAtpPolicyQuery query, CancellationToken cancellationToken) {
        return await _safePoliciesService.GetAtpPolicyAsync(query.TenantId, cancellationToken);
    }
}
