using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IAutopilotService {
    Task<List<AutopilotDeviceDto>> GetDevicesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<AutopilotDeviceDto?> GetDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SyncDevicesAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
