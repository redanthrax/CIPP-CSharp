using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetApplicationsQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetApplicationsQuery, Task<PagedResponse<ApplicationDto>>>;
