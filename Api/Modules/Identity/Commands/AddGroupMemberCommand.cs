using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record AddGroupMemberCommand(
    string TenantId,
    string GroupId,
    AddGroupMemberDto MemberData
) : IRequest<AddGroupMemberCommand, Task>;