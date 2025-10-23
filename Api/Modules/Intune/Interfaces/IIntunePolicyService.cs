using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IIntunePolicyService {
    Task<List<IntunePolicyDto>> GetPoliciesAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto?> GetPolicyAsync(string tenantId, string policyId, string urlName, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto> CreatePolicyAsync(string tenantId, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken = default);
    Task<IntunePolicyDto> UpdatePolicyAsync(string tenantId, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken = default);
    Task DeletePolicyAsync(string tenantId, string policyId, string urlName, CancellationToken cancellationToken = default);
}
