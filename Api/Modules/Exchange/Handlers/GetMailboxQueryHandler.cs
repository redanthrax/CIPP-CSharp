using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxQueryHandler : IRequestHandler<GetMailboxQuery, Task<MailboxDetailsDto?>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<MailboxDetailsDto?> Handle(GetMailboxQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxAsync(query.TenantId, query.UserId, cancellationToken);
    }
}
