using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetSharePointSitesQuery(Guid TenantId, string Type, PagingParameters PagingParams)
    : IRequest<GetSharePointSitesQuery, Task<PagedResponse<SharePointSiteDto>>>;
