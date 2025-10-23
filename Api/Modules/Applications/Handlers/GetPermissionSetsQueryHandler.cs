using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetPermissionSetsQueryHandler : IRequestHandler<GetPermissionSetsQuery, Task<PagedResponse<PermissionSetDto>>> {
    private readonly IPermissionSetService _permissionSetService;

    public GetPermissionSetsQueryHandler(IPermissionSetService permissionSetService) {
        _permissionSetService = permissionSetService;
    }

    public async Task<PagedResponse<PermissionSetDto>> Handle(GetPermissionSetsQuery query, CancellationToken cancellationToken) {
        return await _permissionSetService.GetPermissionSetsAsync(query.Paging, cancellationToken);
    }
}
