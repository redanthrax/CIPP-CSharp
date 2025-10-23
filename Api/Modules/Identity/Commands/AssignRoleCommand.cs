using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record AssignRoleCommand(AssignRoleDto AssignRoleDto) : IRequest<AssignRoleCommand, Task>;
