using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class DeleteInboxRuleCommandHandler : IRequestHandler<DeleteInboxRuleCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public DeleteInboxRuleCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(DeleteInboxRuleCommand command, CancellationToken cancellationToken) {
        await _mailboxService.DeleteInboxRuleAsync(command.TenantId, command.MailboxId, command.RuleId, cancellationToken);
    }
}
