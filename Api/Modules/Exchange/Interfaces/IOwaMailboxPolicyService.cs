using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IOwaMailboxPolicyService {
    Task<PagedResponse<OwaMailboxPolicyDto>> GetOwaMailboxPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<OwaMailboxPolicyDto?> GetOwaMailboxPolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default);
    Task UpdateOwaMailboxPolicyAsync(Guid tenantId, string policyId, UpdateOwaMailboxPolicyDto updateDto, CancellationToken cancellationToken = default);
}
