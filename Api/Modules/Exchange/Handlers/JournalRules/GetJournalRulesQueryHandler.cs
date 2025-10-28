using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.JournalRules;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.JournalRules;

public class GetJournalRulesQueryHandler : IRequestHandler<GetJournalRulesQuery, Task<PagedResponse<JournalRuleDto>>> {
    private readonly IJournalRuleService _journalRuleService;
    private readonly ILogger<GetJournalRulesQueryHandler> _logger;

    public GetJournalRulesQueryHandler(
        IJournalRuleService journalRuleService,
        ILogger<GetJournalRulesQueryHandler> logger) {
        _journalRuleService = journalRuleService;
        _logger = logger;
    }

    public async Task<PagedResponse<JournalRuleDto>> Handle(GetJournalRulesQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling GetJournalRulesQuery for tenant {TenantId}", request.TenantId);
        
        var rules = await _journalRuleService.GetJournalRulesAsync(request.TenantId, request.PagingParams, cancellationToken);
        
        _logger.LogInformation("Retrieved {Count} journal rules for tenant {TenantId}", rules.TotalCount, request.TenantId);
        
        return rules;
    }
}
