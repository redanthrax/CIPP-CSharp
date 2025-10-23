using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record DeleteDeviceCommand(
    string TenantId,
    string DeviceId
) : IRequest<DeleteDeviceCommand, Task>;