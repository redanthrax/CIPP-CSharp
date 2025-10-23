using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Commands;

public record DeleteSharePointSiteCommand(string TenantId, string SiteId) : IRequest<DeleteSharePointSiteCommand, Task>;
