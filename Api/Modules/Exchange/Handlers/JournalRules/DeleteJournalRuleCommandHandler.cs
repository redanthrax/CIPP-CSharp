using CIPP.Api.Modules.Exchange.Commands.JournalRules;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.JournalRules;

public class DeleteJournalRuleCommandHandler : IRequestHandler<DeleteJournalRuleCommand, Task> {
    private readonly IJournalRuleService _journalRuleService;
    private readonly ILogger<DeleteJournalRuleCommandHandler> _logger;

    public DeleteJournalRuleCommandHandler(
        IJournalRuleService journalRuleService,
        ILogger<DeleteJournalRuleCommandHandler> logger) {
        _journalRuleService = journalRuleService;
        _logger = logger;
    }

    public async Task Handle(DeleteJournalRuleCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling DeleteJournalRuleCommand for tenant {TenantId}", request.TenantId);
        
        await _journalRuleService.DeleteJournalRuleAsync(request.TenantId, request.RuleName, cancellationToken);
        
        _logger.LogInformation("Successfully deleted journal rule for tenant {TenantId}", request.TenantId);
    }
}
