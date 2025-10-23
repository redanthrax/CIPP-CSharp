using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class RetireDeviceCommandHandler : IRequestHandler<RetireDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public RetireDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(RetireDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.RetireDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
