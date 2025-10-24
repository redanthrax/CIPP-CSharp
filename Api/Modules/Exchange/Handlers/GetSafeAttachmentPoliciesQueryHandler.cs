using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetSafeAttachmentPoliciesQueryHandler : IRequestHandler<GetSafeAttachmentPoliciesQuery, Task<PagedResponse<SafeAttachmentsPolicyDto>>> {
    private readonly ISafePoliciesService _safePoliciesService;

    public GetSafeAttachmentPoliciesQueryHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task<PagedResponse<SafeAttachmentsPolicyDto>> Handle(GetSafeAttachmentPoliciesQuery query, CancellationToken cancellationToken) {
        return await _safePoliciesService.GetSafeAttachmentPoliciesAsync(query.TenantId, query.PagingParameters, cancellationToken);
    }
}
