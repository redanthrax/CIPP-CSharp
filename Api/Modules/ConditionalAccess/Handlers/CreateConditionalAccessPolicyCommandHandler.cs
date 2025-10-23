using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class CreateConditionalAccessPolicyCommandHandler : IRequestHandler<CreateConditionalAccessPolicyCommand, Task<ConditionalAccessPolicyDto>> {
    private readonly IConditionalAccessPolicyService _policyService;

    public CreateConditionalAccessPolicyCommandHandler(IConditionalAccessPolicyService policyService) {
        _policyService = policyService;
    }

    public async Task<ConditionalAccessPolicyDto> Handle(CreateConditionalAccessPolicyCommand command, CancellationToken cancellationToken) {
        return await _policyService.CreatePolicyAsync(command.Policy, cancellationToken);
    }
}
