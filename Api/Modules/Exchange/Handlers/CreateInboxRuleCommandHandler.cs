using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class CreateInboxRuleCommandHandler : IRequestHandler<CreateInboxRuleCommand, Task<string>> {
    private readonly IMailboxService _mailboxService;

    public CreateInboxRuleCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<string> Handle(CreateInboxRuleCommand command, CancellationToken cancellationToken) {
        return await _mailboxService.CreateInboxRuleAsync(command.TenantId, command.MailboxId, command.CreateDto, cancellationToken);
    }
}
