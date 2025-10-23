using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record ResetPasscodeCommand(string TenantId, string DeviceId) 
    : IRequest<ResetPasscodeCommand, Task>;
