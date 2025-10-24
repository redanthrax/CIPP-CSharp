using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IAuthenticationMethodPolicyService {
    Task<AuthenticationMethodPolicyDto?> GetPolicyAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<AuthenticationMethodDto?> GetMethodConfigAsync(string tenantId, string methodId, CancellationToken cancellationToken = default);
    Task<AuthenticationMethodDto> UpdateMethodConfigAsync(string tenantId, string methodId, UpdateAuthenticationMethodDto updateDto, CancellationToken cancellationToken = default);
}
