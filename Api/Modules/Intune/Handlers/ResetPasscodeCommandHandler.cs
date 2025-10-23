using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class ResetPasscodeCommandHandler : IRequestHandler<ResetPasscodeCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public ResetPasscodeCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(ResetPasscodeCommand command, CancellationToken cancellationToken) {
        await _deviceService.ResetPasscodeAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
