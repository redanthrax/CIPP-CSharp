using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Commands;

public record SetSharePointPermissionsCommand(string TenantId, string UserId, string AccessUser, string? Url, bool RemovePermission) : IRequest<SetSharePointPermissionsCommand, Task<string>>;
