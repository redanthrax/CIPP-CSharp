using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxStatisticsQueryHandler : IRequestHandler<GetMailboxStatisticsQuery, Task<MailboxStatisticsDto?>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxStatisticsQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<MailboxStatisticsDto?> Handle(GetMailboxStatisticsQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxStatisticsAsync(query.TenantId, query.MailboxId, cancellationToken);
    }
}
