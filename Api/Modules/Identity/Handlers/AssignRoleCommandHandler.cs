using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Task> {
    private readonly IRoleService _roleService;
    private readonly ILogger<AssignRoleCommandHandler> _logger;

    public AssignRoleCommandHandler(IRoleService roleService, ILogger<AssignRoleCommandHandler> logger) {
        _roleService = roleService;
        _logger = logger;
    }

    public async Task Handle(AssignRoleCommand command, CancellationToken cancellationToken) {
        _logger.LogInformation("Assigning role {RoleId} to user {UserId} in tenant {TenantId}",
            command.AssignRoleDto.RoleId, command.AssignRoleDto.UserId, command.AssignRoleDto.TenantId);

        await _roleService.AssignRoleAsync(
            command.AssignRoleDto.TenantId,
            command.AssignRoleDto,
            cancellationToken);
    }
}
