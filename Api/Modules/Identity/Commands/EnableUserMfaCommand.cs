using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record EnableUserMfaCommand(
    string TenantId,
    string UserId
) : IRequest<EnableUserMfaCommand, Task>;