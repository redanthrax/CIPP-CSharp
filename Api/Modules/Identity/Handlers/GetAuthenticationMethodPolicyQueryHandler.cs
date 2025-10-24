using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetAuthenticationMethodPolicyQueryHandler : IRequestHandler<GetAuthenticationMethodPolicyQuery, Task<AuthenticationMethodPolicyDto?>> {
    private readonly IAuthenticationMethodPolicyService _authenticationMethodPolicyService;

    public GetAuthenticationMethodPolicyQueryHandler(IAuthenticationMethodPolicyService authenticationMethodPolicyService) {
        _authenticationMethodPolicyService = authenticationMethodPolicyService;
    }

    public async Task<AuthenticationMethodPolicyDto?> Handle(GetAuthenticationMethodPolicyQuery query, CancellationToken cancellationToken) {
        return await _authenticationMethodPolicyService.GetPolicyAsync(query.TenantId, cancellationToken);
    }
}
