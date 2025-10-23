using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxCommandHandler : IRequestHandler<UpdateMailboxCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateMailboxCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateMailboxCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateMailboxAsync(command.TenantId, command.UserId, command.UpdateDto, cancellationToken);
    }
}
