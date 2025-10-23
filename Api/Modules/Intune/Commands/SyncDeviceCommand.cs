using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record SyncDeviceCommand(string TenantId, string DeviceId) 
    : IRequest<SyncDeviceCommand, Task>;
