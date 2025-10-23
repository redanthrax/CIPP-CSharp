using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DeletePermissionSetCommandHandler : IRequestHandler<DeletePermissionSetCommand, Task> {
    private readonly IPermissionSetService _permissionSetService;

    public DeletePermissionSetCommandHandler(IPermissionSetService permissionSetService) {
        _permissionSetService = permissionSetService;
    }

    public async Task Handle(DeletePermissionSetCommand command, CancellationToken cancellationToken) {
        await _permissionSetService.DeletePermissionSetAsync(command.Id, cancellationToken);
    }
}
