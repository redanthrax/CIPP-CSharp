using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class DeleteConditionalAccessPolicyCommandHandler : IRequestHandler<DeleteConditionalAccessPolicyCommand, Task> {
    private readonly IConditionalAccessPolicyService _policyService;

    public DeleteConditionalAccessPolicyCommandHandler(IConditionalAccessPolicyService policyService) {
        _policyService = policyService;
    }

    public async Task Handle(DeleteConditionalAccessPolicyCommand command, CancellationToken cancellationToken) {
        await _policyService.DeletePolicyAsync(command.TenantId, command.PolicyId, cancellationToken);
    }
}
