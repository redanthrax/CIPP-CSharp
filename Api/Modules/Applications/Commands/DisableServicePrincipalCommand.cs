using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record DisableServicePrincipalCommand(
    string TenantId,
    string ServicePrincipalId
) : IRequest<DisableServicePrincipalCommand, Task>;
