using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IDeviceService {
    Task<PagedResponse<DeviceDto>> GetDevicesAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<DeviceDto?> GetDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DisableDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task EnableDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceDto>> GetUserDevicesAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
}