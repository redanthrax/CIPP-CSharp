using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxCalendarConfigurationQueryHandler : IRequestHandler<GetMailboxCalendarConfigurationQuery, Task<MailboxCalendarConfigurationDto?>> {
    private readonly IMailboxService _mailboxService;
    private readonly ILogger<GetMailboxCalendarConfigurationQueryHandler> _logger;

    public GetMailboxCalendarConfigurationQueryHandler(IMailboxService mailboxService, ILogger<GetMailboxCalendarConfigurationQueryHandler> logger) {
        _mailboxService = mailboxService;
        _logger = logger;
    }

    public async Task<MailboxCalendarConfigurationDto?> Handle(GetMailboxCalendarConfigurationQuery query, CancellationToken cancellationToken) {
        _logger.LogInformation("Getting calendar configuration for mailbox {MailboxId}", query.MailboxId);
        return await _mailboxService.GetMailboxCalendarConfigurationAsync(query.TenantId, query.MailboxId, cancellationToken);
    }
}
