using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetAppConsentRequestsQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetAppConsentRequestsQuery, Task<PagedResponse<AppConsentRequestDto>>>;
