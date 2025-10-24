using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateLitigationHoldCommandHandler : IRequestHandler<UpdateLitigationHoldCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateLitigationHoldCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateLitigationHoldCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateLitigationHoldAsync(command.TenantId, command.MailboxId, command.HoldDto, cancellationToken);
    }
}
