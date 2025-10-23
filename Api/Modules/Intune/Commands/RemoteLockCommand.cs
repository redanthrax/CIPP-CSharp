using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record RemoteLockCommand(string TenantId, string DeviceId) 
    : IRequest<RemoteLockCommand, Task>;
