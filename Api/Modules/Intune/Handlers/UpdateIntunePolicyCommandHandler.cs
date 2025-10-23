using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class UpdateIntunePolicyCommandHandler : IRequestHandler<UpdateIntunePolicyCommand, Task<IntunePolicyDto>> {
    private readonly IIntunePolicyService _policyService;

    public UpdateIntunePolicyCommandHandler(IIntunePolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<IntunePolicyDto> Handle(UpdateIntunePolicyCommand command, CancellationToken cancellationToken) {
        return await _policyService.UpdatePolicyAsync(command.TenantId, command.PolicyId, command.PolicyDto, cancellationToken);
    }
}
