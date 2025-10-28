using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.ActiveSync;

public class UpdateActiveSyncDeviceAccessRuleCommandHandler : IRequestHandler<UpdateActiveSyncDeviceAccessRuleCommand, Task> {
    private readonly IActiveSyncService _activeSyncService;

    public UpdateActiveSyncDeviceAccessRuleCommandHandler(IActiveSyncService activeSyncService) {
        _activeSyncService = activeSyncService;
    }

    public async Task Handle(UpdateActiveSyncDeviceAccessRuleCommand command, CancellationToken cancellationToken) {
        await _activeSyncService.UpdateDeviceAccessRuleAsync(command.TenantId, command.RuleIdentity, command.UpdateDto, cancellationToken);
    }
}