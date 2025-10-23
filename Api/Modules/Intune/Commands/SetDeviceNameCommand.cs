using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record SetDeviceNameCommand(string TenantId, string DeviceId, string DeviceName) 
    : IRequest<SetDeviceNameCommand, Task>;
