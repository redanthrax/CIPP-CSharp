using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;

namespace CIPP.Api.Modules.ConditionalAccess.Interfaces;

public interface IConditionalAccessPolicyService {
    Task<PagedResponse<ConditionalAccessPolicyDto>> GetPoliciesAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<ConditionalAccessPolicyDto?> GetPolicyAsync(string tenantId, string policyId, CancellationToken cancellationToken = default);
    Task<ConditionalAccessPolicyDto> CreatePolicyAsync(CreateConditionalAccessPolicyDto createDto, CancellationToken cancellationToken = default);
    Task<ConditionalAccessPolicyDto> UpdatePolicyAsync(string tenantId, string policyId, UpdateConditionalAccessPolicyDto updateDto, CancellationToken cancellationToken = default);
    Task DeletePolicyAsync(string tenantId, string policyId, CancellationToken cancellationToken = default);
}
