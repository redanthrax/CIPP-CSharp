using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecureScoreService {
    Task<List<SecureScoreControlProfileDto>> GetControlProfilesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<SecureScoreControlProfileDto?> GetControlProfileAsync(Guid tenantId, string controlName, CancellationToken cancellationToken = default);
    Task UpdateControlProfileAsync(Guid tenantId, string controlName, UpdateSecureScoreControlDto updateDto, CancellationToken cancellationToken = default);
}
