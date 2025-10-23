using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record SyncAutopilotDevicesCommand(string TenantId) 
    : IRequest<SyncAutopilotDevicesCommand, Task>;
