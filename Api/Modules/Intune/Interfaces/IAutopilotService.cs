using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IAutopilotService {
    Task<List<AutopilotDeviceDto>> GetDevicesAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<AutopilotDeviceDto?> GetDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SyncDevicesAsync(string tenantId, CancellationToken cancellationToken = default);
}
