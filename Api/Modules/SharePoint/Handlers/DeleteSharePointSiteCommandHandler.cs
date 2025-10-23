using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class DeleteSharePointSiteCommandHandler : IRequestHandler<DeleteSharePointSiteCommand, Task> {
    private readonly ISharePointSiteService _siteService;

    public DeleteSharePointSiteCommandHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task Handle(DeleteSharePointSiteCommand command, CancellationToken cancellationToken) {
        await _siteService.DeleteSiteAsync(command.TenantId, command.SiteId, cancellationToken);
    }
}
