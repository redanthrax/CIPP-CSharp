using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetSharePointQuotaQueryHandler : IRequestHandler<GetSharePointQuotaQuery, Task<SharePointQuotaDto>> {
    private readonly ISharePointSiteService _siteService;

    public GetSharePointQuotaQueryHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<SharePointQuotaDto> Handle(GetSharePointQuotaQuery query, CancellationToken cancellationToken) {
        return await _siteService.GetQuotaAsync(query.TenantId, cancellationToken);
    }
}
