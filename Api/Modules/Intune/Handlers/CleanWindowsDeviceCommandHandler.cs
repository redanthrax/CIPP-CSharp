using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class CleanWindowsDeviceCommandHandler : IRequestHandler<CleanWindowsDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public CleanWindowsDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(CleanWindowsDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.CleanWindowsDeviceAsync(command.TenantId, command.DeviceId, command.KeepUserData, cancellationToken);
    }
}
