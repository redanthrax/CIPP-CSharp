using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record CleanWindowsDeviceCommand(string TenantId, string DeviceId, bool KeepUserData) 
    : IRequest<CleanWindowsDeviceCommand, Task>;
