using CIPP.Api.Modules.Exchange.Commands.Mailboxes;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Mailboxes;

public class ConvertToSharedMailboxCommandHandler : IRequestHandler<ConvertToSharedMailboxCommand, Task> {
    private readonly IMailboxService _mailboxService;

    public ConvertToSharedMailboxCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task Handle(ConvertToSharedMailboxCommand request, CancellationToken cancellationToken = default) {
        await _mailboxService.ConvertToSharedMailboxAsync(request.TenantId, request.Identity, cancellationToken);
    }
}
