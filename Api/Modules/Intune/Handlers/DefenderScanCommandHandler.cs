using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class DefenderScanCommandHandler : IRequestHandler<DefenderScanCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public DefenderScanCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(DefenderScanCommand command, CancellationToken cancellationToken) {
        await _deviceService.DefenderScanAsync(command.TenantId, command.DeviceId, command.QuickScan, cancellationToken);
    }
}
