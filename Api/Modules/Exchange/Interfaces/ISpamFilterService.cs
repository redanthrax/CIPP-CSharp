using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ISpamFilterService {
    Task<PagedResponse<HostedContentFilterPolicyDto>> GetAntiSpamPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<HostedContentFilterPolicyDto?> GetAntiSpamPolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default);
    Task UpdateAntiSpamPolicyAsync(Guid tenantId, string policyId, UpdateHostedContentFilterPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<MalwareFilterPolicyDto>> GetAntiMalwarePoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<MalwareFilterPolicyDto?> GetAntiMalwarePolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default);
    Task UpdateAntiMalwarePolicyAsync(Guid tenantId, string policyId, UpdateMalwareFilterPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<QuarantineMessageDto>> GetQuarantineMessagesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task ReleaseQuarantineMessageAsync(Guid tenantId, string messageId, CancellationToken cancellationToken = default);
    Task DeleteQuarantineMessageAsync(Guid tenantId, string messageId, CancellationToken cancellationToken = default);
}
