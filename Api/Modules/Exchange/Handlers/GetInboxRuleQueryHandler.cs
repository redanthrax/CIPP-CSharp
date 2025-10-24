using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetInboxRuleQueryHandler : IRequestHandler<GetInboxRuleQuery, Task<InboxRuleDto?>> {
    private readonly IMailboxService _mailboxService;

    public GetInboxRuleQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<InboxRuleDto?> Handle(GetInboxRuleQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetInboxRuleAsync(query.TenantId, query.MailboxId, query.RuleId, cancellationToken);
    }
}
