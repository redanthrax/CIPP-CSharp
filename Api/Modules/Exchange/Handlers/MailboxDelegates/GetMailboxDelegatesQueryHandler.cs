using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MailboxDelegates;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailboxDelegates;

public class GetMailboxDelegatesQueryHandler : IRequestHandler<GetMailboxDelegatesQuery, Task<List<MailboxDelegateDto>>> {
    private readonly IMailboxDelegateService _mailboxDelegateService;
    private readonly ILogger<GetMailboxDelegatesQueryHandler> _logger;

    public GetMailboxDelegatesQueryHandler(
        IMailboxDelegateService mailboxDelegateService,
        ILogger<GetMailboxDelegatesQueryHandler> logger) {
        _mailboxDelegateService = mailboxDelegateService;
        _logger = logger;
    }

    public async Task<List<MailboxDelegateDto>> Handle(GetMailboxDelegatesQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling GetMailboxDelegatesQuery for mailbox {MailboxId} in tenant {TenantId}", 
            request.MailboxId, request.TenantId);
        
        var delegates = await _mailboxDelegateService.GetMailboxDelegatesAsync(
            request.TenantId, 
            request.MailboxId, 
            cancellationToken);
        
        _logger.LogInformation("Retrieved {Count} delegates for mailbox {MailboxId}", 
            delegates.Count, request.MailboxId);
        
        return delegates;
    }
}
