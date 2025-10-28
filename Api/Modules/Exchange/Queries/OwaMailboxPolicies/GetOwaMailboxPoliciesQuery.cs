using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;

public record GetOwaMailboxPoliciesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetOwaMailboxPoliciesQuery, Task<PagedResponse<OwaMailboxPolicyDto>>>;
