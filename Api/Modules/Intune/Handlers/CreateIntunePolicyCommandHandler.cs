using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class CreateIntunePolicyCommandHandler : IRequestHandler<CreateIntunePolicyCommand, Task<IntunePolicyDto>> {
    private readonly IIntunePolicyService _policyService;

    public CreateIntunePolicyCommandHandler(IIntunePolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<IntunePolicyDto> Handle(CreateIntunePolicyCommand command, CancellationToken cancellationToken) {
        return await _policyService.CreatePolicyAsync(command.TenantId, command.PolicyDto, cancellationToken);
    }
}
