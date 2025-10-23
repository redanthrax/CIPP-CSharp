using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, Task> {
    private readonly IRoleService _roleService;
    private readonly ILogger<RemoveRoleCommandHandler> _logger;

    public RemoveRoleCommandHandler(IRoleService roleService, ILogger<RemoveRoleCommandHandler> logger) {
        _roleService = roleService;
        _logger = logger;
    }

    public async Task Handle(RemoveRoleCommand command, CancellationToken cancellationToken) {
        _logger.LogInformation("Removing role {RoleId} from user {UserId} in tenant {TenantId}",
            command.RoleId, command.UserId, command.TenantId);

        await _roleService.RemoveRoleAsync(
            command.TenantId,
            command.UserId,
            command.RoleId,
            cancellationToken);
    }
}
