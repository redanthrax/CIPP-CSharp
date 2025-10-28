using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.Mailboxes;

public record ConvertToSharedMailboxCommand(Guid TenantId, string Identity)
    : IRequest<ConvertToSharedMailboxCommand, Task>;
