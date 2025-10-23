using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class WipeDeviceCommandHandler : IRequestHandler<WipeDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public WipeDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(WipeDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.WipeDeviceAsync(command.TenantId, command.DeviceId, command.KeepEnrollmentData, command.KeepUserData, command.UseProtectedWipe, cancellationToken);
    }
}
