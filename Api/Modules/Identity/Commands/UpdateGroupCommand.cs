using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record UpdateGroupCommand(
    string TenantId,
    string GroupId,
    UpdateGroupDto GroupData
) : IRequest<UpdateGroupCommand, Task<GroupDto>>;