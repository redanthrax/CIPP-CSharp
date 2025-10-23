using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class GetIntunePoliciesQueryHandler : IRequestHandler<GetIntunePoliciesQuery, Task<List<IntunePolicyDto>>> {
    private readonly IIntunePolicyService _policyService;

    public GetIntunePoliciesQueryHandler(IIntunePolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<List<IntunePolicyDto>> Handle(GetIntunePoliciesQuery query, CancellationToken cancellationToken) {
        return await _policyService.GetPoliciesAsync(query.TenantId, cancellationToken);
    }
}
