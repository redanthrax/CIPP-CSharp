using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class RotateLocalAdminPasswordCommandHandler : IRequestHandler<RotateLocalAdminPasswordCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public RotateLocalAdminPasswordCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(RotateLocalAdminPasswordCommand command, CancellationToken cancellationToken) {
        await _deviceService.RotateLocalAdminPasswordAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
