using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetSharePointSiteQuery(string TenantId, string SiteId) : IRequest<GetSharePointSiteQuery, Task<SharePointSiteDto?>>;
