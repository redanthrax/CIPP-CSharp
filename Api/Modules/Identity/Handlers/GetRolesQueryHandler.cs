using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Task<PagedResponse<RoleDto>>> {
    private readonly IRoleService _roleService;
    private readonly ILogger<GetRolesQueryHandler> _logger;

    public GetRolesQueryHandler(
        IRoleService roleService,
        ILogger<GetRolesQueryHandler> logger) {
        _roleService = roleService;
        _logger = logger;
    }

    public async Task<PagedResponse<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Getting roles for tenant {TenantId}", request.TenantId);
            
            var result = await _roleService.GetRolesAsync(request.TenantId, request.Paging, cancellationToken);
            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get roles for tenant {TenantId}", request.TenantId);
            return new PagedResponse<RoleDto> {
                Items = new List<RoleDto>(),
                TotalCount = 0,
                PageNumber = request.Paging?.PageNumber ?? 1,
                PageSize = request.Paging?.PageSize ?? 50
            };
        }
    }
}
