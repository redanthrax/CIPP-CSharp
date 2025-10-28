using CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailboxDelegates;

public class RemoveMailboxDelegateCommandHandler : IRequestHandler<RemoveMailboxDelegateCommand, Task> {
    private readonly IMailboxDelegateService _mailboxDelegateService;
    private readonly ILogger<RemoveMailboxDelegateCommandHandler> _logger;

    public RemoveMailboxDelegateCommandHandler(
        IMailboxDelegateService mailboxDelegateService,
        ILogger<RemoveMailboxDelegateCommandHandler> logger) {
        _mailboxDelegateService = mailboxDelegateService;
        _logger = logger;
    }

    public async Task Handle(RemoveMailboxDelegateCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling RemoveMailboxDelegateCommand for mailbox {MailboxId} in tenant {TenantId}", 
            request.MailboxId, request.TenantId);
        
        await _mailboxDelegateService.RemoveMailboxDelegateAsync(
            request.TenantId, 
            request.MailboxId, 
            request.DelegateData, 
            cancellationToken);
        
        _logger.LogInformation("Successfully removed delegate from mailbox {MailboxId}", request.MailboxId);
    }
}
