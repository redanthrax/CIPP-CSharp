using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Queries;

public record GetIntunePolicyQuery(string TenantId, string PolicyId, string UrlName) 
    : IRequest<GetIntunePolicyQuery, Task<IntunePolicyDto?>>;
