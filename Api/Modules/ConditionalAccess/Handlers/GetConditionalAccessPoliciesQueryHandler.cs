using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetConditionalAccessPoliciesQueryHandler : IRequestHandler<GetConditionalAccessPoliciesQuery, Task<PagedResponse<ConditionalAccessPolicyDto>>> {
    private readonly IConditionalAccessPolicyService _policyService;

    public GetConditionalAccessPoliciesQueryHandler(IConditionalAccessPolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<PagedResponse<ConditionalAccessPolicyDto>> Handle(GetConditionalAccessPoliciesQuery query, CancellationToken cancellationToken) {
        return await _policyService.GetPoliciesAsync(query.TenantId, query.Paging, cancellationToken);
    }
}
