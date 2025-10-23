using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class GetIntunePolicyQueryHandler : IRequestHandler<GetIntunePolicyQuery, Task<IntunePolicyDto?>> {
    private readonly IIntunePolicyService _policyService;

    public GetIntunePolicyQueryHandler(IIntunePolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<IntunePolicyDto?> Handle(GetIntunePolicyQuery query, CancellationToken cancellationToken) {
        return await _policyService.GetPolicyAsync(query.TenantId, query.PolicyId, query.UrlName, cancellationToken);
    }
}
