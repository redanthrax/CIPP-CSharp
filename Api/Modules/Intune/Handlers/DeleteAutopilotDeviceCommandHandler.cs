using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class DeleteAutopilotDeviceCommandHandler : IRequestHandler<DeleteAutopilotDeviceCommand, Task> {
    private readonly IAutopilotService _autopilotService;

    public DeleteAutopilotDeviceCommandHandler(IAutopilotService autopilotService) {
        _autopilotService = autopilotService;
    }

    public async Task Handle(DeleteAutopilotDeviceCommand command, CancellationToken cancellationToken) {
        await _autopilotService.DeleteDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
