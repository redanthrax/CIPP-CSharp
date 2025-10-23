using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record DisableUserMfaCommand(
    string TenantId,
    string UserId
) : IRequest<DisableUserMfaCommand, Task>;