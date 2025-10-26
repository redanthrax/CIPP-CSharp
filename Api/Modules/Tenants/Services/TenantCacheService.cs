using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CIPP.Api.Modules.Tenants.Services;

public class TenantCacheService : ITenantCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<TenantCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    
    private const string TENANT_KEY_PREFIX = "tenant:";
    private const string TENANTS_LIST_KEY = "tenants:list";
    private const string TENANTS_COUNT_KEY = "tenants:count";
    
    public TenantCacheService(IDistributedCache cache, ILogger<TenantCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<Tenant?> GetTenantAsync(Guid tenantId)
    {
        try
        {
            var key = $"{TENANT_KEY_PREFIX}{tenantId}";
            var json = await _cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;
                
            return JsonSerializer.Deserialize<Tenant>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get tenant {TenantId} from cache", tenantId);
            return null;
        }
    }

    public async Task<Tenant?> GetTenantByTenantIdAsync(Guid tenantId)
    {
        return await GetTenantAsync(tenantId);
    }

    public async Task<List<Tenant>> GetTenantsAsync(string? filter = null, int skip = 0, int take = 50)
    {
        try
        {
            var key = $"{TENANTS_LIST_KEY}:{filter ?? "all"}:{skip}:{take}";
            var json = await _cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return new List<Tenant>();
                
            return JsonSerializer.Deserialize<List<Tenant>>(json, _jsonOptions) ?? new List<Tenant>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get tenants list from cache");
            return new List<Tenant>();
        }
    }

    public async Task SetTenantAsync(Tenant tenant, TimeSpan? expiry = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(tenant, _jsonOptions);
            var options = new DistributedCacheEntryOptions();
            
            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            
            var key = $"{TENANT_KEY_PREFIX}{tenant.TenantId}";
            
            await _cache.SetStringAsync(key, json, options);
            
            await InvalidateTenantsListCache();
            
            _logger.LogDebug("Cached tenant {TenantId}", tenant.TenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache tenant {TenantId}", tenant.TenantId);
        }
    }

    public async Task RemoveTenantAsync(Guid tenantId)
    {
        try
        {
            var key = $"{TENANT_KEY_PREFIX}{tenantId}";
            await _cache.RemoveAsync(key);
            
            await InvalidateTenantsListCache();
            
            _logger.LogDebug("Removed tenant {TenantId} from cache", tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove tenant {TenantId} from cache", tenantId);
        }
    }

    public async Task RemoveTenantByTenantIdAsync(Guid tenantId)
    {
        await RemoveTenantAsync(tenantId);
    }

    public async Task InvalidateTenantsListCache()
    {
        try
        {
            await _cache.RemoveAsync(TENANTS_LIST_KEY);
            await _cache.RemoveAsync(TENANTS_COUNT_KEY);
            
            _logger.LogDebug("Invalidated tenants list cache");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to invalidate tenants list cache");
        }
    }

    public async Task<int> GetTenantCountAsync(string? filter = null)
    {
        try
        {
            var key = $"{TENANTS_COUNT_KEY}:{filter ?? "all"}";
            var countStr = await _cache.GetStringAsync(key);
            
            if (int.TryParse(countStr, out var count))
                return count;
                
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get tenant count from cache");
            return 0;
        }
    }

    public async Task SetTenantCountAsync(int count, string? filter = null, TimeSpan? expiry = null)
    {
        try
        {
            var key = $"{TENANTS_COUNT_KEY}:{filter ?? "all"}";
            var options = new DistributedCacheEntryOptions();
            
            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetSlidingExpiration(TimeSpan.FromMinutes(15));
            
            await _cache.SetStringAsync(key, count.ToString(), options);
            
            _logger.LogDebug("Cached tenant count {Count} for filter {Filter}", count, filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache tenant count");
        }
    }

    public async Task SetTenantsListAsync(List<Tenant> tenants, string? filter = null, int skip = 0, int take = 50, TimeSpan? expiry = null)
    {
        try
        {
            var key = $"{TENANTS_LIST_KEY}:{filter ?? "all"}:{skip}:{take}";
            var json = JsonSerializer.Serialize(tenants, _jsonOptions);
            var options = new DistributedCacheEntryOptions();
            
            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetSlidingExpiration(TimeSpan.FromMinutes(10));
            
            await _cache.SetStringAsync(key, json, options);
            
            _logger.LogDebug("Cached tenants list with {Count} items", tenants.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache tenants list");
        }
    }
}
