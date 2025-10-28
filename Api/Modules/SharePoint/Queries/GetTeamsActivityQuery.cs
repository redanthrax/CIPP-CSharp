using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamsActivityQuery(Guid TenantId, string Type, PagingParameters PagingParams)
    : IRequest<GetTeamsActivityQuery, Task<PagedResponse<TeamsActivityDto>>>;
