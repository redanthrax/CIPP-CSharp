using CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailboxDelegates;

public class AddMailboxDelegateCommandHandler : IRequestHandler<AddMailboxDelegateCommand, Task> {
    private readonly IMailboxDelegateService _mailboxDelegateService;
    private readonly ILogger<AddMailboxDelegateCommandHandler> _logger;

    public AddMailboxDelegateCommandHandler(
        IMailboxDelegateService mailboxDelegateService,
        ILogger<AddMailboxDelegateCommandHandler> logger) {
        _mailboxDelegateService = mailboxDelegateService;
        _logger = logger;
    }

    public async Task Handle(AddMailboxDelegateCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling AddMailboxDelegateCommand for mailbox {MailboxId} in tenant {TenantId}", 
            request.MailboxId, request.TenantId);
        
        await _mailboxDelegateService.AddMailboxDelegateAsync(
            request.TenantId, 
            request.MailboxId, 
            request.DelegateData, 
            cancellationToken);
        
        _logger.LogInformation("Successfully added delegate to mailbox {MailboxId}", request.MailboxId);
    }
}
