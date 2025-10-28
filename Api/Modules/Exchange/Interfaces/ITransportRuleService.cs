using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ITransportRuleService {
    Task<PagedResponse<TransportRuleDto>> GetTransportRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<TransportRuleDetailsDto?> GetTransportRuleAsync(Guid tenantId, string ruleId, CancellationToken cancellationToken = default);
    Task<string> CreateTransportRuleAsync(Guid tenantId, CreateTransportRuleDto createDto, CancellationToken cancellationToken = default);
    Task UpdateTransportRuleAsync(Guid tenantId, string ruleId, UpdateTransportRuleDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteTransportRuleAsync(Guid tenantId, string ruleId, CancellationToken cancellationToken = default);
    Task EnableTransportRuleAsync(Guid tenantId, string ruleId, bool enable, CancellationToken cancellationToken = default);
}
