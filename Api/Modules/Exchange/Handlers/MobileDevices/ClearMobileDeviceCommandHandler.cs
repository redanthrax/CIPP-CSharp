using CIPP.Api.Modules.Exchange.Commands.MobileDevices;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MobileDevices;

public class ClearMobileDeviceCommandHandler : IRequestHandler<ClearMobileDeviceCommand, Task> {
    private readonly IMobileDeviceService _mobileDeviceService;

    public ClearMobileDeviceCommandHandler(IMobileDeviceService mobileDeviceService) {
        _mobileDeviceService = mobileDeviceService;
    }

    public async Task Handle(ClearMobileDeviceCommand command, CancellationToken cancellationToken) {
        await _mobileDeviceService.ClearMobileDeviceAsync(command.TenantId, command.DeviceId, command.ClearDto, cancellationToken);
    }
}