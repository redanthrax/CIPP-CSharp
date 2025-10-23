using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxCommand(string TenantId, string UserId, UpdateMailboxDto UpdateDto) : IRequest<UpdateMailboxCommand, Task>;
