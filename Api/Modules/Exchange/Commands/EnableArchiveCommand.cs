using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record EnableArchiveCommand(string TenantId, string MailboxId) : IRequest<EnableArchiveCommand, Task>;
