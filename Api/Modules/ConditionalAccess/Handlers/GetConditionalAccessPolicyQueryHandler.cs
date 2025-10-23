using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetConditionalAccessPolicyQueryHandler : IRequestHandler<GetConditionalAccessPolicyQuery, Task<ConditionalAccessPolicyDto?>> {
    private readonly IConditionalAccessPolicyService _policyService;

    public GetConditionalAccessPolicyQueryHandler(IConditionalAccessPolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<ConditionalAccessPolicyDto?> Handle(GetConditionalAccessPolicyQuery query, CancellationToken cancellationToken) {
        return await _policyService.GetPolicyAsync(query.TenantId, query.PolicyId, cancellationToken);
    }
}
