using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class CreateSharePointSiteCommandHandler : IRequestHandler<CreateSharePointSiteCommand, Task<string>> {
    private readonly ISharePointSiteService _siteService;

    public CreateSharePointSiteCommandHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<string> Handle(CreateSharePointSiteCommand command, CancellationToken cancellationToken) {
        return await _siteService.CreateSiteAsync(command.TenantId, command.CreateDto, cancellationToken);
    }
}
