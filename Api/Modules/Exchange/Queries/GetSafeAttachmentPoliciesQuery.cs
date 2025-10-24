using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetSafeAttachmentPoliciesQuery(string TenantId, PagingParameters PagingParameters) : IRequest<GetSafeAttachmentPoliciesQuery, Task<PagedResponse<SafeAttachmentsPolicyDto>>>;
