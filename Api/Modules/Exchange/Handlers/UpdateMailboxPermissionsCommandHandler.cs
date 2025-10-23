using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxPermissionsCommandHandler : IRequestHandler<UpdateMailboxPermissionsCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public UpdateMailboxPermissionsCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(UpdateMailboxPermissionsCommand command, CancellationToken cancellationToken) {
        await _mailboxService.UpdateMailboxPermissionsAsync(command.TenantId, command.UserId, command.UpdateDto, cancellationToken);
    }
}
