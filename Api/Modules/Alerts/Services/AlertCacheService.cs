using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Shared.DTOs.Alerts;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertCacheService : IAlertCacheService {
    private readonly IDistributedCache _cache;
    private readonly ILogger<AlertCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    
    private const string ALERT_CONFIGURATIONS_KEY = "alert_configurations";
    
    public AlertCacheService(IDistributedCache cache, ILogger<AlertCacheService> logger) {
        _cache = cache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<List<AlertConfigurationDto>> GetAlertConfigurationsAsync() {
        try {
            var json = await _cache.GetStringAsync(ALERT_CONFIGURATIONS_KEY);
            
            if (string.IsNullOrEmpty(json))
                return new List<AlertConfigurationDto>();
                
            return JsonSerializer.Deserialize<List<AlertConfigurationDto>>(json, _jsonOptions) ?? 
                   new List<AlertConfigurationDto>();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get alert configurations from cache");
            return new List<AlertConfigurationDto>();
        }
    }

    public async Task SetAlertConfigurationsAsync(List<AlertConfigurationDto> alerts, TimeSpan? expiry = null) {
        try {
            var json = JsonSerializer.Serialize(alerts, _jsonOptions);
            var options = new DistributedCacheEntryOptions();
            
            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetSlidingExpiration(TimeSpan.FromMinutes(15));
            
            await _cache.SetStringAsync(ALERT_CONFIGURATIONS_KEY, json, options);
            
            _logger.LogDebug("Cached {Count} alert configurations", alerts.Count);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to cache alert configurations");
        }
    }

    public async Task InvalidateAlertConfigurationsCache() {
        try {
            await _cache.RemoveAsync(ALERT_CONFIGURATIONS_KEY);
            
            _logger.LogDebug("Invalidated alert configurations cache");
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invalidate alert configurations cache");
        }
    }
}