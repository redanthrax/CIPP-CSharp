using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetServicePrincipalsQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetServicePrincipalsQuery, Task<PagedResponse<ServicePrincipalDto>>>;
