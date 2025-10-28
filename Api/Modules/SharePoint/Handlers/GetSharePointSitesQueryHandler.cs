using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetSharePointSitesQueryHandler : IRequestHandler<GetSharePointSitesQuery, Task<PagedResponse<SharePointSiteDto>>> {
    private readonly ISharePointSiteService _siteService;

    public GetSharePointSitesQueryHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<PagedResponse<SharePointSiteDto>> Handle(GetSharePointSitesQuery query, CancellationToken cancellationToken) {
        return await _siteService.GetSitesAsync(query.TenantId, query.Type, query.PagingParams, cancellationToken);
    }
}
