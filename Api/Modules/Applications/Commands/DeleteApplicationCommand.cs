using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record DeleteApplicationCommand(
    string TenantId,
    string ApplicationId
) : IRequest<DeleteApplicationCommand, Task>;
