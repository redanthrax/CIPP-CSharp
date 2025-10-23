using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ITransportRuleService {
    Task<List<TransportRuleDto>> GetTransportRulesAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<TransportRuleDetailsDto?> GetTransportRuleAsync(string tenantId, string ruleId, CancellationToken cancellationToken = default);
    Task<string> CreateTransportRuleAsync(string tenantId, CreateTransportRuleDto createDto, CancellationToken cancellationToken = default);
    Task UpdateTransportRuleAsync(string tenantId, string ruleId, UpdateTransportRuleDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteTransportRuleAsync(string tenantId, string ruleId, CancellationToken cancellationToken = default);
    Task EnableTransportRuleAsync(string tenantId, string ruleId, bool enable, CancellationToken cancellationToken = default);
}
