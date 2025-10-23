using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetPermissionSetQueryHandler : IRequestHandler<GetPermissionSetQuery, Task<PermissionSetDto?>> {
    private readonly IPermissionSetService _permissionSetService;

    public GetPermissionSetQueryHandler(IPermissionSetService permissionSetService) {
        _permissionSetService = permissionSetService;
    }

    public async Task<PermissionSetDto?> Handle(GetPermissionSetQuery query, CancellationToken cancellationToken) {
        return await _permissionSetService.GetPermissionSetAsync(query.Id, cancellationToken);
    }
}
