using CIPP.Api.Modules.Exchange.Commands.JournalRules;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.JournalRules;

public class CreateJournalRuleCommandHandler : IRequestHandler<CreateJournalRuleCommand, Task<string>> {
    private readonly IJournalRuleService _journalRuleService;
    private readonly ILogger<CreateJournalRuleCommandHandler> _logger;

    public CreateJournalRuleCommandHandler(
        IJournalRuleService journalRuleService,
        ILogger<CreateJournalRuleCommandHandler> logger) {
        _journalRuleService = journalRuleService;
        _logger = logger;
    }

    public async Task<string> Handle(CreateJournalRuleCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling CreateJournalRuleCommand for tenant {TenantId}", request.TenantId);
        
        var result = await _journalRuleService.CreateJournalRuleAsync(request.TenantId, request.RuleData, cancellationToken);
        
        _logger.LogInformation("Successfully created journal rule for tenant {TenantId}", request.TenantId);
        
        return result;
    }
}
