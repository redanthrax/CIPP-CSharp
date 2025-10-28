using CIPP.Api.Modules.Exchange.Commands.Mailboxes;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Mailboxes;

public class CreateSharedMailboxCommandHandler : IRequestHandler<CreateSharedMailboxCommand, Task<string>> {
    private readonly IMailboxService _mailboxService;

    public CreateSharedMailboxCommandHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<string> Handle(CreateSharedMailboxCommand request, CancellationToken cancellationToken = default) {
        return await _mailboxService.CreateSharedMailboxAsync(request.TenantId, request.CreateDto, cancellationToken);
    }
}
