using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class UpdatePermissionSetCommandHandler : IRequestHandler<UpdatePermissionSetCommand, Task<PermissionSetDto>> {
    private readonly IPermissionSetService _permissionSetService;

    public UpdatePermissionSetCommandHandler(IPermissionSetService permissionSetService) {
        _permissionSetService = permissionSetService;
    }

    public async Task<PermissionSetDto> Handle(UpdatePermissionSetCommand command, CancellationToken cancellationToken) {
        return await _permissionSetService.UpdatePermissionSetAsync(command.Id, command.UpdateDto, cancellationToken);
    }
}
