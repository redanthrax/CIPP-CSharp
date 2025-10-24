using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetAuthenticationMethodConfigQueryHandler : IRequestHandler<GetAuthenticationMethodConfigQuery, Task<AuthenticationMethodDto?>> {
    private readonly IAuthenticationMethodPolicyService _authenticationMethodPolicyService;

    public GetAuthenticationMethodConfigQueryHandler(IAuthenticationMethodPolicyService authenticationMethodPolicyService) {
        _authenticationMethodPolicyService = authenticationMethodPolicyService;
    }

    public async Task<AuthenticationMethodDto?> Handle(GetAuthenticationMethodConfigQuery query, CancellationToken cancellationToken) {
        return await _authenticationMethodPolicyService.GetMethodConfigAsync(query.TenantId, query.MethodId, cancellationToken);
    }
}
