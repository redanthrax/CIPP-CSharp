using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record DeleteServicePrincipalCommand(
    string TenantId,
    string ServicePrincipalId
) : IRequest<DeleteServicePrincipalCommand, Task>;
