using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxForwardingCommand(string TenantId, string UserId, UpdateMailboxForwardingDto UpdateDto) : IRequest<UpdateMailboxForwardingCommand, Task>;
