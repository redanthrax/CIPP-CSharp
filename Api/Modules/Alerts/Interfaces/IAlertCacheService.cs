using CIPP.Shared.DTOs.Alerts;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertCacheService {
    Task<List<AlertConfigurationDto>> GetAlertConfigurationsAsync();
    Task SetAlertConfigurationsAsync(List<AlertConfigurationDto> alerts, TimeSpan? expiry = null);
    Task InvalidateAlertConfigurationsCache();
}