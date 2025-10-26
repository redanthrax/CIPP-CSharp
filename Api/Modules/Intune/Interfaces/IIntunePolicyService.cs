using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IIntunePolicyService {
    Task<List<IntunePolicyDto>> GetPoliciesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto?> GetPolicyAsync(Guid tenantId, string policyId, string urlName, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto> CreatePolicyAsync(Guid tenantId, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto> UpdatePolicyAsync(Guid tenantId, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken = default);
    Task DeletePolicyAsync(Guid tenantId, string policyId, string urlName, CancellationToken cancellationToken = default);
}
