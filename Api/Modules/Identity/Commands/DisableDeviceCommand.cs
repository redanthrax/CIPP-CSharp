using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record DisableDeviceCommand(string TenantId, string DeviceId) : IRequest<DisableDeviceCommand, Task>;