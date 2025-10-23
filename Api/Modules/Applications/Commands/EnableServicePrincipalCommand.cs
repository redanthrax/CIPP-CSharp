using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record EnableServicePrincipalCommand(
    string TenantId,
    string ServicePrincipalId
) : IRequest<EnableServicePrincipalCommand, Task>;
