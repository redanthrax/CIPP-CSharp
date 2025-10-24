using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateInboxRuleCommandHandler : IRequestHandler<UpdateInboxRuleCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateInboxRuleCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateInboxRuleCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateInboxRuleAsync(command.TenantId, command.MailboxId, command.RuleId, command.UpdateDto, cancellationToken);
    }
}
