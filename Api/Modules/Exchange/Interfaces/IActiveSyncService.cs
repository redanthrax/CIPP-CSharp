using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IActiveSyncService {
    Task<PagedResponse<ActiveSyncDeviceAccessRuleDto>> GetDeviceAccessRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<ActiveSyncDeviceAccessRuleDto?> GetDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, CancellationToken cancellationToken = default);
    Task<string> CreateDeviceAccessRuleAsync(Guid tenantId, CreateActiveSyncDeviceAccessRuleDto createDto, CancellationToken cancellationToken = default);
    Task UpdateDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, UpdateActiveSyncDeviceAccessRuleDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, CancellationToken cancellationToken = default);
}
