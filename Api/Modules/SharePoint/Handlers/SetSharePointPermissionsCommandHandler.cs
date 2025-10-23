using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class SetSharePointPermissionsCommandHandler : IRequestHandler<SetSharePointPermissionsCommand, Task<string>> {
    private readonly ISharePointSiteService _siteService;

    public SetSharePointPermissionsCommandHandler(ISharePointSiteService siteService) {
        _siteService = siteService;
    }

    public async Task<string> Handle(SetSharePointPermissionsCommand command, CancellationToken cancellationToken) {
        return await _siteService.SetPermissionsAsync(
            command.TenantId,
            command.UserId,
            command.AccessUser,
            command.Url,
            command.RemovePermission,
            cancellationToken
        );
    }
}
