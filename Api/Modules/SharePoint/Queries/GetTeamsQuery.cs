using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamsQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetTeamsQuery, Task<PagedResponse<TeamDto>>>;
