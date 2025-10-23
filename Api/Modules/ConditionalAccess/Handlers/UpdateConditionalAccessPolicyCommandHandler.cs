using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class UpdateConditionalAccessPolicyCommandHandler : IRequestHandler<UpdateConditionalAccessPolicyCommand, Task<ConditionalAccessPolicyDto>> {
    private readonly IConditionalAccessPolicyService _policyService;

    public UpdateConditionalAccessPolicyCommandHandler(IConditionalAccessPolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<ConditionalAccessPolicyDto> Handle(UpdateConditionalAccessPolicyCommand command, CancellationToken cancellationToken) {
        return await _policyService.UpdatePolicyAsync(command.TenantId, command.PolicyId, command.Policy, cancellationToken);
    }
}
