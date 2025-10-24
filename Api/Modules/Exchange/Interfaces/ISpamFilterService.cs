using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ISpamFilterService {
    Task<PagedResponse<HostedContentFilterPolicyDto>> GetAntiSpamPoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<HostedContentFilterPolicyDto?> GetAntiSpamPolicyAsync(string tenantId, string policyId, CancellationToken cancellationToken = default);
    Task UpdateAntiSpamPolicyAsync(string tenantId, string policyId, UpdateHostedContentFilterPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<MalwareFilterPolicyDto>> GetAntiMalwarePoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<MalwareFilterPolicyDto?> GetAntiMalwarePolicyAsync(string tenantId, string policyId, CancellationToken cancellationToken = default);
    Task UpdateAntiMalwarePolicyAsync(string tenantId, string policyId, UpdateMalwareFilterPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<QuarantineMessageDto>> GetQuarantineMessagesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task ReleaseQuarantineMessageAsync(string tenantId, string messageId, CancellationToken cancellationToken = default);
    Task DeleteQuarantineMessageAsync(string tenantId, string messageId, CancellationToken cancellationToken = default);
}
