using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class SyncAutopilotDevicesCommandHandler : IRequestHandler<SyncAutopilotDevicesCommand, Task> {
    private readonly IAutopilotService _autopilotService;

    public SyncAutopilotDevicesCommandHandler(IAutopilotService autopilotService) {
        _autopilotService = autopilotService;
    }

    public async Task Handle(SyncAutopilotDevicesCommand command, CancellationToken cancellationToken) {
        await _autopilotService.SyncDevicesAsync(command.TenantId, cancellationToken);
    }
}
