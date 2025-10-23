using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class DeleteIntunePolicyCommandHandler : IRequestHandler<DeleteIntunePolicyCommand, Task> {
    private readonly IIntunePolicyService _policyService;

    public DeleteIntunePolicyCommandHandler(IIntunePolicyService policyService) {
        _policyService = policyService;
    }

    public async Task Handle(DeleteIntunePolicyCommand command, CancellationToken cancellationToken) {
        await _policyService.DeletePolicyAsync(command.TenantId, command.PolicyId, command.UrlName, cancellationToken);
    }
}
