using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class MobileDeviceService : IMobileDeviceService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<MobileDeviceService> _logger;

    public MobileDeviceService(IExchangeOnlineService exoService, ILogger<MobileDeviceService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<MobileDeviceDto>> GetMobileDevicesAsync(Guid tenantId, string? mailbox, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mobile devices for tenant {TenantId}, mailbox {Mailbox}", tenantId, mailbox ?? "all");
        
        var parameters = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(mailbox)) {
            parameters.Add("Mailbox", mailbox);
        }

        var devices = await _exoService.ExecuteCmdletListAsync<MobileDeviceDto>(
            tenantId,
            "Get-MobileDevice",
            parameters.Count > 0 ? parameters : null,
            cancellationToken
        );

        foreach (var device in devices) {
            device.TenantId = tenantId;
        }

        return devices.ToPagedResponse(pagingParams);
    }

    public async Task<MobileDeviceDto?> GetMobileDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mobile device {DeviceId} for tenant {TenantId}", deviceId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", deviceId }
        };

        var device = await _exoService.ExecuteCmdletAsync<MobileDeviceDto>(
            tenantId,
            "Get-MobileDevice",
            parameters,
            cancellationToken
        );

        if (device != null) {
            device.TenantId = tenantId;
        }

        return device;
    }

    public async Task<MobileDeviceStatisticsDto?> GetMobileDeviceStatisticsAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mobile device statistics for {DeviceId} in tenant {TenantId}", deviceId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", deviceId }
        };

        var statistics = await _exoService.ExecuteCmdletAsync<MobileDeviceStatisticsDto>(
            tenantId,
            "Get-MobileDeviceStatistics",
            parameters,
            cancellationToken
        );

        if (statistics != null) {
            statistics.TenantId = tenantId;
        }

        return statistics;
    }

    public async Task RemoveMobileDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Removing mobile device {DeviceId} from tenant {TenantId}", deviceId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", deviceId },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Remove-MobileDevice",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully removed mobile device {DeviceId}", deviceId);
    }

    public async Task ClearMobileDeviceAsync(Guid tenantId, string deviceId, ClearMobileDeviceDto clearDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Clearing mobile device {DeviceId} for tenant {TenantId}, Cancel={Cancel}", deviceId, tenantId, clearDto.Cancel);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", deviceId },
            { "Cancel", clearDto.Cancel },
            { "Confirm", false }
        };

        if (!string.IsNullOrEmpty(clearDto.NotificationEmailAddresses)) {
            parameters.Add("NotificationEmailAddresses", clearDto.NotificationEmailAddresses.Split(','));
        }

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Clear-MobileDevice",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully {Action} mobile device clear for {DeviceId}", 
            clearDto.Cancel ? "cancelled" : "initiated", deviceId);
    }
}
