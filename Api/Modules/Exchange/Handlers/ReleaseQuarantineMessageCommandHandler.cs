using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class ReleaseQuarantineMessageCommandHandler : IRequestHandler<ReleaseQuarantineMessageCommand, Task> {
    private readonly ISpamFilterService _spamFilterService;

    public ReleaseQuarantineMessageCommandHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task Handle(ReleaseQuarantineMessageCommand command, CancellationToken cancellationToken) {
        await _spamFilterService.ReleaseQuarantineMessageAsync(command.TenantId, command.MessageId, cancellationToken);
    }
}
