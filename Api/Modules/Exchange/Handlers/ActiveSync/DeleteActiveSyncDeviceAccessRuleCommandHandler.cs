using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.ActiveSync;

public class DeleteActiveSyncDeviceAccessRuleCommandHandler : IRequestHandler<DeleteActiveSyncDeviceAccessRuleCommand, Task> {
    private readonly IActiveSyncService _activeSyncService;

    public DeleteActiveSyncDeviceAccessRuleCommandHandler(IActiveSyncService activeSyncService) {
        _activeSyncService = activeSyncService;
    }

    public async Task Handle(DeleteActiveSyncDeviceAccessRuleCommand command, CancellationToken cancellationToken) {
        await _activeSyncService.DeleteDeviceAccessRuleAsync(command.TenantId, command.RuleIdentity, cancellationToken);
    }
}