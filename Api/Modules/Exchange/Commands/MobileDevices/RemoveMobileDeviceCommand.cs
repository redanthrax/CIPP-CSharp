using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MobileDevices;

public record RemoveMobileDeviceCommand(Guid TenantId, string DeviceId)
    : IRequest<RemoveMobileDeviceCommand, Task>;