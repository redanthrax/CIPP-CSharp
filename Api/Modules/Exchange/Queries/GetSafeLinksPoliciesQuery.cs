using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetSafeLinksPoliciesQuery(string TenantId, PagingParameters PagingParameters) : IRequest<GetSafeLinksPoliciesQuery, Task<PagedResponse<SafeLinksPolicyDto>>>;
