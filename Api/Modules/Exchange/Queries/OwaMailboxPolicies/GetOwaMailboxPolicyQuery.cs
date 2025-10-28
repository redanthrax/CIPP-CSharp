using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;

public record GetOwaMailboxPolicyQuery(Guid TenantId, string PolicyId)
    : IRequest<GetOwaMailboxPolicyQuery, Task<OwaMailboxPolicyDto?>>;
