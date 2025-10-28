using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;

public record RemoveMailboxDelegateCommand(Guid TenantId, string MailboxId, RemoveMailboxDelegateDto DelegateData)
    : IRequest<RemoveMailboxDelegateCommand, Task>;
