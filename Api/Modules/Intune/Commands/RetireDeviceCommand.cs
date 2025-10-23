using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record RetireDeviceCommand(string TenantId, string DeviceId) 
    : IRequest<RetireDeviceCommand, Task>;
