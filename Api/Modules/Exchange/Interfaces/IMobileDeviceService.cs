using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMobileDeviceService {
    Task<PagedResponse<MobileDeviceDto>> GetMobileDevicesAsync(Guid tenantId, string? mailbox, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<MobileDeviceDto?> GetMobileDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task<MobileDeviceStatisticsDto?> GetMobileDeviceStatisticsAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task RemoveMobileDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task ClearMobileDeviceAsync(Guid tenantId, string deviceId, ClearMobileDeviceDto clearDto, CancellationToken cancellationToken = default);
}
