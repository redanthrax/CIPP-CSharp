using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record LocateDeviceCommand(string TenantId, string DeviceId) 
    : IRequest<LocateDeviceCommand, Task>;
