using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.Mailboxes;

public record CreateSharedMailboxCommand(Guid TenantId, CreateSharedMailboxDto CreateDto)
    : IRequest<CreateSharedMailboxCommand, Task<string>>;
