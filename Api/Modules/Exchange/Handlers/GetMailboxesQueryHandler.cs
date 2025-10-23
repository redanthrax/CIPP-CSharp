using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxesQueryHandler : IRequestHandler<GetMailboxesQuery, Task<List<MailboxDto>>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxesQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<List<MailboxDto>> Handle(GetMailboxesQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxesAsync(query.TenantId, query.MailboxType, cancellationToken);
    }
}
