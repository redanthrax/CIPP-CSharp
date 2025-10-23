using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxForwardingQueryHandler : IRequestHandler<GetMailboxForwardingQuery, Task<MailboxForwardingDto>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxForwardingQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<MailboxForwardingDto> Handle(GetMailboxForwardingQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxForwardingAsync(query.TenantId, query.UserId, cancellationToken);
    }
}
