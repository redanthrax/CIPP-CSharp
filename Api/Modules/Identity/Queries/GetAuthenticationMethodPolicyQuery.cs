using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetAuthenticationMethodPolicyQuery(string TenantId) : IRequest<GetAuthenticationMethodPolicyQuery, Task<AuthenticationMethodPolicyDto?>>;
