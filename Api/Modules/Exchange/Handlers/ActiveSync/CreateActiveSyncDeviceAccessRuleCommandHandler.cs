using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.ActiveSync;

public class CreateActiveSyncDeviceAccessRuleCommandHandler : IRequestHandler<CreateActiveSyncDeviceAccessRuleCommand, Task<string>> {
    private readonly IActiveSyncService _activeSyncService;

    public CreateActiveSyncDeviceAccessRuleCommandHandler(IActiveSyncService activeSyncService) {
        _activeSyncService = activeSyncService;
    }

    public async Task<string> Handle(CreateActiveSyncDeviceAccessRuleCommand command, CancellationToken cancellationToken) {
        return await _activeSyncService.CreateDeviceAccessRuleAsync(command.TenantId, command.CreateDto, cancellationToken);
    }
}