using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record RemoveRoleCommand(string TenantId, string RoleId, string UserId) : IRequest<RemoveRoleCommand, Task>;
