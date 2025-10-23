using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record RemoveGroupMemberCommand(
    string TenantId,
    string GroupId,
    string UserId
) : IRequest<RemoveGroupMemberCommand, Task>;