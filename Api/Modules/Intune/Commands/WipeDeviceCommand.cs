using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record WipeDeviceCommand(string TenantId, string DeviceId, bool KeepEnrollmentData, bool KeepUserData, bool UseProtectedWipe) 
    : IRequest<WipeDeviceCommand, Task>;
