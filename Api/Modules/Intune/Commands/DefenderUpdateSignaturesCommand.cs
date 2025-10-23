using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record DefenderUpdateSignaturesCommand(string TenantId, string DeviceId) 
    : IRequest<DefenderUpdateSignaturesCommand, Task>;
