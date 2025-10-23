using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecureScoreService {
    Task<List<SecureScoreControlProfileDto>> GetControlProfilesAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<SecureScoreControlProfileDto?> GetControlProfileAsync(string tenantId, string controlName, CancellationToken cancellationToken = default);
    Task UpdateControlProfileAsync(string tenantId, string controlName, UpdateSecureScoreControlDto updateDto, CancellationToken cancellationToken = default);
}
