using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetUsersQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetUsersQuery, Task<PagedResponse<UserDto>>>;
