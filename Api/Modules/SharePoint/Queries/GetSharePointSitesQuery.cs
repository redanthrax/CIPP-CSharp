using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetSharePointSitesQuery(string TenantId, string Type) : IRequest<GetSharePointSitesQuery, Task<List<SharePointSiteDto>>>;
