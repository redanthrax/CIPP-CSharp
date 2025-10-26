using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ISafePoliciesService {
    Task<PagedResponse<SafeLinksPolicyDto>> GetSafeLinksPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<SafeLinksPolicyDto?> GetSafeLinksPolicyAsync(Guid tenantId, string policyName, CancellationToken cancellationToken = default);
    Task UpdateSafeLinksPolicyAsync(Guid tenantId, string policyName, UpdateSafeLinksPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<SafeAttachmentsPolicyDto>> GetSafeAttachmentPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<SafeAttachmentsPolicyDto?> GetSafeAttachmentPolicyAsync(Guid tenantId, string policyName, CancellationToken cancellationToken = default);
    Task UpdateSafeAttachmentPolicyAsync(Guid tenantId, string policyName, UpdateSafeAttachmentsPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<AtpPolicyDto?> GetAtpPolicyAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpdateAtpPolicyAsync(Guid tenantId, UpdateAtpPolicyDto updateDto, CancellationToken cancellationToken = default);
}
