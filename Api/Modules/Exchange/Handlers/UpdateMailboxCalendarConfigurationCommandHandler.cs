using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxCalendarConfigurationCommandHandler : IRequestHandler<UpdateMailboxCalendarConfigurationCommand, Task> {
    private readonly IMailboxService _mailboxService;
    private readonly ILogger<UpdateMailboxCalendarConfigurationCommandHandler> _logger;

    public UpdateMailboxCalendarConfigurationCommandHandler(IMailboxService mailboxService, ILogger<UpdateMailboxCalendarConfigurationCommandHandler> logger) {
        _mailboxService = mailboxService;
        _logger = logger;
    }

    public async Task Handle(UpdateMailboxCalendarConfigurationCommand command, CancellationToken cancellationToken) {
        _logger.LogInformation("Updating calendar configuration for mailbox {MailboxId}", command.MailboxId);
        await _mailboxService.UpdateMailboxCalendarConfigurationAsync(command.TenantId, command.MailboxId, command.UpdateDto, cancellationToken);
    }
}
