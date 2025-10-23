using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class RemoteLockCommandHandler : IRequestHandler<RemoteLockCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public RemoteLockCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(RemoteLockCommand command, CancellationToken cancellationToken) {
        await _deviceService.RemoteLockDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
