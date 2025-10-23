using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecurityAlertService {
    Task<SecurityAlertsResponseDto> GetAlertsAsync(string tenantId, string? serviceSource = null, CancellationToken cancellationToken = default);
    Task<SecurityAlertDto?> GetAlertAsync(string tenantId, string alertId, CancellationToken cancellationToken = default);
    Task UpdateAlertAsync(string tenantId, string alertId, UpdateSecurityAlertDto updateDto, CancellationToken cancellationToken = default);
}
