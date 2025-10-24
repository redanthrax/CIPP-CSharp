using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetAntiSpamPoliciesQuery(string TenantId, PagingParameters PagingParams) : IRequest<GetAntiSpamPoliciesQuery, Task<PagedResponse<HostedContentFilterPolicyDto>>>;
