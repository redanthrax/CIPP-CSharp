using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxForwardingCommandHandler : IRequestHandler<UpdateMailboxForwardingCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateMailboxForwardingCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateMailboxForwardingCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateMailboxForwardingAsync(command.TenantId, command.UserId, command.UpdateDto, cancellationToken);
    }
}
