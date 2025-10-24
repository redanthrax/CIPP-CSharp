using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxQuotaQueryHandler : IRequestHandler<GetMailboxQuotaQuery, Task<MailboxQuotaDto?>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxQuotaQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<MailboxQuotaDto?> Handle(GetMailboxQuotaQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxQuotaAsync(query.TenantId, query.MailboxId, cancellationToken);
    }
}
