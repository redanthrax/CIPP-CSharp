using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetRolesQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetRolesQuery, Task<PagedResponse<RoleDto>>>;
