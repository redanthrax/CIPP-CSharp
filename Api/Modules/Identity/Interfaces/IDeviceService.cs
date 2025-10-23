using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IDeviceService {
    Task<PagedResponse<DeviceDto>> GetDevicesAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<DeviceDto?> GetDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DisableDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task EnableDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceDto>> GetUserDevicesAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
}