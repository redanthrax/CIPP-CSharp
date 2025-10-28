using CIPP.Api.Modules.Exchange.Commands.MobileDevices;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MobileDevices;

public class RemoveMobileDeviceCommandHandler : IRequestHandler<RemoveMobileDeviceCommand, Task> {
    private readonly IMobileDeviceService _mobileDeviceService;

    public RemoveMobileDeviceCommandHandler(IMobileDeviceService mobileDeviceService) {
        _mobileDeviceService = mobileDeviceService;
    }

    public async Task Handle(RemoveMobileDeviceCommand command, CancellationToken cancellationToken) {
        await _mobileDeviceService.RemoveMobileDeviceAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}