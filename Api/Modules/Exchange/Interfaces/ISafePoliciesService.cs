using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ISafePoliciesService {
    Task<PagedResponse<SafeLinksPolicyDto>> GetSafeLinksPoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<SafeLinksPolicyDto?> GetSafeLinksPolicyAsync(string tenantId, string policyName, CancellationToken cancellationToken = default);
    Task UpdateSafeLinksPolicyAsync(string tenantId, string policyName, UpdateSafeLinksPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<SafeAttachmentsPolicyDto>> GetSafeAttachmentPoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<SafeAttachmentsPolicyDto?> GetSafeAttachmentPolicyAsync(string tenantId, string policyName, CancellationToken cancellationToken = default);
    Task UpdateSafeAttachmentPolicyAsync(string tenantId, string policyName, UpdateSafeAttachmentsPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task<AtpPolicyDto?> GetAtpPolicyAsync(string tenantId, CancellationToken cancellationToken = default);
    Task UpdateAtpPolicyAsync(string tenantId, UpdateAtpPolicyDto updateDto, CancellationToken cancellationToken = default);
}
