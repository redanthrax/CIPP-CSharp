using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class UpdateAuthenticationMethodConfigCommandHandler : IRequestHandler<UpdateAuthenticationMethodConfigCommand, Task<AuthenticationMethodDto>> {
    private readonly IAuthenticationMethodPolicyService _authenticationMethodPolicyService;

    public UpdateAuthenticationMethodConfigCommandHandler(IAuthenticationMethodPolicyService authenticationMethodPolicyService) {
        _authenticationMethodPolicyService = authenticationMethodPolicyService;
    }

    public async Task<AuthenticationMethodDto> Handle(UpdateAuthenticationMethodConfigCommand command, CancellationToken cancellationToken) {
        return await _authenticationMethodPolicyService.UpdateMethodConfigAsync(command.TenantId, command.MethodId, command.UpdateDto, cancellationToken);
    }
}
