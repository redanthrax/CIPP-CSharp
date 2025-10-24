using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetSafeAttachmentPolicyQueryHandler : IRequestHandler<GetSafeAttachmentPolicyQuery, Task<SafeAttachmentsPolicyDto?>> {
    private readonly ISafePoliciesService _safePoliciesService;

    public GetSafeAttachmentPolicyQueryHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task<SafeAttachmentsPolicyDto?> Handle(GetSafeAttachmentPolicyQuery query, CancellationToken cancellationToken) {
        return await _safePoliciesService.GetSafeAttachmentPolicyAsync(query.TenantId, query.PolicyName, cancellationToken);
    }
}
