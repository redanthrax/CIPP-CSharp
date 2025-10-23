using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Queries;

public record GetIntunePoliciesQuery(string TenantId) 
    : IRequest<GetIntunePoliciesQuery, Task<List<IntunePolicyDto>>>;
