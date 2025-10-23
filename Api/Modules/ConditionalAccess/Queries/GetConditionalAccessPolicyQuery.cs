using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Queries;

public record GetConditionalAccessPolicyQuery(
    string TenantId,
    string PolicyId
) : IRequest<GetConditionalAccessPolicyQuery, Task<ConditionalAccessPolicyDto?>>;
