using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IAuthenticationMethodPolicyService {
    Task<AuthenticationMethodPolicyDto?> GetPolicyAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<AuthenticationMethodDto?> GetMethodConfigAsync(Guid tenantId, string methodId, CancellationToken cancellationToken = default);
    Task<AuthenticationMethodDto> UpdateMethodConfigAsync(Guid tenantId, string methodId, UpdateAuthenticationMethodDto updateDto, CancellationToken cancellationToken = default);
}
