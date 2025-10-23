using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record DeleteUserCommand(
    string TenantId,
    string UserId
) : IRequest<DeleteUserCommand, Task>;