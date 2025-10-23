using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetSharePointSiteQueryHandler : IRequestHandler<GetSharePointSiteQuery, Task<SharePointSiteDto?>> {
    private readonly ISharePointSiteService _siteService;

    public GetSharePointSiteQueryHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<SharePointSiteDto?> Handle(GetSharePointSiteQuery query, CancellationToken cancellationToken) {
        return await _siteService.GetSiteAsync(query.TenantId, query.SiteId, cancellationToken);
    }
}
