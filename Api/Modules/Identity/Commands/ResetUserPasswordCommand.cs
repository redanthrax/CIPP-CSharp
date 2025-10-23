using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record ResetUserPasswordCommand(
    string TenantId,
    string UserId,
    ResetUserPasswordDto PasswordData
) : IRequest<ResetUserPasswordCommand, Task<string>>;