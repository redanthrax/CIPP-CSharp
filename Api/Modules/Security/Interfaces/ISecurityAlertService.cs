using CIPP.Shared.DTOs.Security;

namespace CIPP.Api.Modules.Security.Interfaces;

public interface ISecurityAlertService {
    Task<SecurityAlertsResponseDto> GetAlertsAsync(Guid tenantId, string? serviceSource = null, CancellationToken cancellationToken = default);
    Task<SecurityAlertDto?> GetAlertAsync(Guid tenantId, string alertId, CancellationToken cancellationToken = default);
    Task UpdateAlertAsync(Guid tenantId, string alertId, UpdateSecurityAlertDto updateDto, CancellationToken cancellationToken = default);
}
