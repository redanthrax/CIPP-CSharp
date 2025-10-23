using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record CreateGroupCommand(
    CreateGroupDto GroupData
) : IRequest<CreateGroupCommand, Task<GroupDto>>;