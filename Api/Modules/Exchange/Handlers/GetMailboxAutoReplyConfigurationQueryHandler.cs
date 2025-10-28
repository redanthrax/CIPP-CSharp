using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxAutoReplyConfigurationQueryHandler : IRequestHandler<GetMailboxAutoReplyConfigurationQuery, Task<MailboxAutoReplyConfigurationDto?>> {
    private readonly IMailboxService _mailboxService;
    private readonly ILogger<GetMailboxAutoReplyConfigurationQueryHandler> _logger;

    public GetMailboxAutoReplyConfigurationQueryHandler(IMailboxService mailboxService, ILogger<GetMailboxAutoReplyConfigurationQueryHandler> logger) {
        _mailboxService = mailboxService;
        _logger = logger;
    }

    public async Task<MailboxAutoReplyConfigurationDto?> Handle(GetMailboxAutoReplyConfigurationQuery query, CancellationToken cancellationToken) {
        _logger.LogInformation("Getting auto-reply configuration for mailbox {MailboxId}", query.MailboxId);
        return await _mailboxService.GetMailboxAutoReplyConfigurationAsync(query.TenantId, query.MailboxId, cancellationToken);
    }
}
