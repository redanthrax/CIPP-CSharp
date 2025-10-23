using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record DeleteNamedLocationCommand(
    string TenantId,
    string LocationId
) : IRequest<DeleteNamedLocationCommand, Task>;
