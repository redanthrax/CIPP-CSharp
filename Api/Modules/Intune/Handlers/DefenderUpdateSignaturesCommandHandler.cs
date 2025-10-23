using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class DefenderUpdateSignaturesCommandHandler : IRequestHandler<DefenderUpdateSignaturesCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public DefenderUpdateSignaturesCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(DefenderUpdateSignaturesCommand command, CancellationToken cancellationToken) {
        await _deviceService.DefenderUpdateSignaturesAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
