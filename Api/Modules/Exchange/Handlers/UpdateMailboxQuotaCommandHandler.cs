using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxQuotaCommandHandler : IRequestHandler<UpdateMailboxQuotaCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateMailboxQuotaCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateMailboxQuotaCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateMailboxQuotaAsync(command.TenantId, command.MailboxId, command.UpdateDto, cancellationToken);
    }
}
