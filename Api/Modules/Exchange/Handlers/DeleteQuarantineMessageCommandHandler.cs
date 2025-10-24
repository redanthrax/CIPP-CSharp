using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class DeleteQuarantineMessageCommandHandler : IRequestHandler<DeleteQuarantineMessageCommand, Task> {
    private readonly ISpamFilterService _spamFilterService;

    public DeleteQuarantineMessageCommandHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task Handle(DeleteQuarantineMessageCommand command, CancellationToken cancellationToken) {
        await _spamFilterService.DeleteQuarantineMessageAsync(command.TenantId, command.MessageId, cancellationToken);
    }
}
