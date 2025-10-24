using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class EnableArchiveCommandHandler : IRequestHandler<EnableArchiveCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public EnableArchiveCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(EnableArchiveCommand command, CancellationToken cancellationToken) {
        await _mailboxService.EnableArchiveAsync(command.TenantId, command.MailboxId, cancellationToken);
    }
}
