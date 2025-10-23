using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record RebootDeviceCommand(string TenantId, string DeviceId) 
    : IRequest<RebootDeviceCommand, Task>;
