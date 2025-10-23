using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class LocateDeviceCommandHandler : IRequestHandler<LocateDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public LocateDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(LocateDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.LocateDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
