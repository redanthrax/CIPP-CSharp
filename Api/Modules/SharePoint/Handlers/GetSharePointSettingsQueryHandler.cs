using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetSharePointSettingsQueryHandler : IRequestHandler<GetSharePointSettingsQuery, Task<SharePointSettingsDto>> {
    private readonly ISharePointSiteService _siteService;

    public GetSharePointSettingsQueryHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<SharePointSettingsDto> Handle(GetSharePointSettingsQuery query, CancellationToken cancellationToken) {
        return await _siteService.GetSettingsAsync(query.TenantId, cancellationToken);
    }
}
