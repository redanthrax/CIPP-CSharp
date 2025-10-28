using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;

public record AddMailboxDelegateCommand(Guid TenantId, string MailboxId, AddMailboxDelegateDto DelegateData)
    : IRequest<AddMailboxDelegateCommand, Task>;
