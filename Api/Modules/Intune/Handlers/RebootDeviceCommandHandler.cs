using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class RebootDeviceCommandHandler : IRequestHandler<RebootDeviceCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public RebootDeviceCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(RebootDeviceCommand command, CancellationToken cancellationToken) {
        await _deviceService.RebootDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
