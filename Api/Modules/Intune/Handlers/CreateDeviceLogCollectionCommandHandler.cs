using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class CreateDeviceLogCollectionCommandHandler : IRequestHandler<CreateDeviceLogCollectionCommand, Task> {
    private readonly IManagedDeviceService _deviceService;

    public CreateDeviceLogCollectionCommandHandler(IManagedDeviceService deviceService) {
        _deviceService = deviceService;
    }

    public async Task Handle(CreateDeviceLogCollectionCommand command, CancellationToken cancellationToken) {
        await _deviceService.CreateDeviceLogCollectionAsync(command.TenantId, command.DeviceId, cancellationToken);
    }
}
