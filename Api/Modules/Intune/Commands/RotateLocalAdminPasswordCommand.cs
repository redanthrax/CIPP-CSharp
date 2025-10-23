using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record RotateLocalAdminPasswordCommand(string TenantId, string DeviceId) 
    : IRequest<RotateLocalAdminPasswordCommand, Task>;
