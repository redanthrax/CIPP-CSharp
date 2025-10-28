using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateMailboxAutoReplyConfigurationCommandHandler : IRequestHandler<UpdateMailboxAutoReplyConfigurationCommand, Task> {
    private readonly IMailboxService _mailboxService;
    private readonly ILogger<UpdateMailboxAutoReplyConfigurationCommandHandler> _logger;

    public UpdateMailboxAutoReplyConfigurationCommandHandler(IMailboxService mailboxService, ILogger<UpdateMailboxAutoReplyConfigurationCommandHandler> logger) {
        _mailboxService = mailboxService;
        _logger = logger;
    }

    public async Task Handle(UpdateMailboxAutoReplyConfigurationCommand command, CancellationToken cancellationToken) {
        _logger.LogInformation("Updating auto-reply configuration for mailbox {MailboxId}", command.MailboxId);
        await _mailboxService.UpdateMailboxAutoReplyConfigurationAsync(command.TenantId, command.MailboxId, command.UpdateDto, cancellationToken);
    }
}
