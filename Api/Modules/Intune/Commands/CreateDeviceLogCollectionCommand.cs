using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record CreateDeviceLogCollectionCommand(string TenantId, string DeviceId) 
    : IRequest<CreateDeviceLogCollectionCommand, Task>;
