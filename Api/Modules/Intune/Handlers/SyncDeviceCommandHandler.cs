using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class SyncDeviceCommandHandler : IRequestHandler<SyncDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public SyncDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(SyncDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.SyncDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
