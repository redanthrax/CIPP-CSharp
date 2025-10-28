using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MailResources;

public record CreateRoomMailboxCommand(Guid TenantId, CreateRoomMailboxDto CreateDto)
    : IRequest<CreateRoomMailboxCommand, Task<string>>;
