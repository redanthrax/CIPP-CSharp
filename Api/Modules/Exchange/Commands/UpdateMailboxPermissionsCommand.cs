using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxPermissionsCommand(string TenantId, string UserId, UpdateMailboxPermissionsDto UpdateDto) : IRequest<UpdateMailboxPermissionsCommand, Task>;
