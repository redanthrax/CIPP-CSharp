using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record DeleteAutopilotDeviceCommand(string TenantId, string DeviceId) 
    : IRequest<DeleteAutopilotDeviceCommand, Task>;
