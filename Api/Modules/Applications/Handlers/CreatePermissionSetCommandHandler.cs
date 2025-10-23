using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class CreatePermissionSetCommandHandler : IRequestHandler<CreatePermissionSetCommand, Task<PermissionSetDto>> {
    private readonly IPermissionSetService _permissionSetService;

    public CreatePermissionSetCommandHandler(IPermissionSetService permissionSetService) {
        _permissionSetService = permissionSetService;
    }

    public async Task<PermissionSetDto> Handle(CreatePermissionSetCommand command, CancellationToken cancellationToken) {
        return await _permissionSetService.CreatePermissionSetAsync(command.CreateDto, cancellationToken);
    }
}
