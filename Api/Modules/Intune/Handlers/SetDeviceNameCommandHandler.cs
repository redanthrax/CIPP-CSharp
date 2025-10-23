using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class SetDeviceNameCommandHandler : IRequestHandler<SetDeviceNameCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public SetDeviceNameCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(SetDeviceNameCommand command, CancellationToken cancellationToken) {
        await _deviceService.SetDeviceNameAsync(command.TenantId, command.DeviceId, command.DeviceName, cancellationToken);
    }
}
