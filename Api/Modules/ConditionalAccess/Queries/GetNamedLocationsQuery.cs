using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Queries;

public record GetNamedLocationsQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetNamedLocationsQuery, Task<PagedResponse<NamedLocationDto>>>;
