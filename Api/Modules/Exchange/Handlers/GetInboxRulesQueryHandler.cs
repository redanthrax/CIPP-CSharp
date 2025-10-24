using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetInboxRulesQueryHandler : IRequestHandler<GetInboxRulesQuery, Task<PagedResponse<InboxRuleDto>>> {
    private readonly IMailboxService _mailboxService;

    public GetInboxRulesQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<PagedResponse<InboxRuleDto>> Handle(GetInboxRulesQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetInboxRulesAsync(query.TenantId, query.MailboxId, query.PagingParams, cancellationToken);
    }
}
