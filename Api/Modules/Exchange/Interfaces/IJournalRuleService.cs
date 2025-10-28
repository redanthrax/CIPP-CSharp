using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IJournalRuleService {
    Task<PagedResponse<JournalRuleDto>> GetJournalRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<string> CreateJournalRuleAsync(Guid tenantId, CreateJournalRuleDto createDto, CancellationToken cancellationToken = default);
    Task DeleteJournalRuleAsync(Guid tenantId, string ruleName, CancellationToken cancellationToken = default);
}
