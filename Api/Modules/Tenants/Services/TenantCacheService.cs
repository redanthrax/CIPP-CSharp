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
    private const string TENANT_BY_TENANT_ID_PREFIX = "tenant_by_id:";
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

    public async Task<Tenant?> GetTenantAsync(Guid id)
    {
        try
        {
            var key = $"{TENANT_KEY_PREFIX}{id}";
            var json = await _cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;
                
            return JsonSerializer.Deserialize<Tenant>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get tenant {Id} from cache", id);
            return null;
        }
    }

    public async Task<Tenant?> GetTenantByTenantIdAsync(string tenantId)
    {
        try
        {
            var key = $"{TENANT_BY_TENANT_ID_PREFIX}{tenantId}";
            var json = await _cache.GetStringAsync(key);
            
            if (string.IsNullOrEmpty(json))
                return null;
                
            return JsonSerializer.Deserialize<Tenant>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get tenant by tenantId {TenantId} from cache", tenantId);
            return null;
        }
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
            
            var idKey = $"{TENANT_KEY_PREFIX}{tenant.Id}";
            var tenantIdKey = $"{TENANT_BY_TENANT_ID_PREFIX}{tenant.TenantId}";
            
            await _cache.SetStringAsync(idKey, json, options);
            await _cache.SetStringAsync(tenantIdKey, json, options);
            
            await InvalidateTenantsListCache();
            
            _logger.LogDebug("Cached tenant {Id} with tenantId {TenantId}", tenant.Id, tenant.TenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache tenant {Id}", tenant.Id);
        }
    }

    public async Task RemoveTenantAsync(Guid id)
    {
        try
        {
            var key = $"{TENANT_KEY_PREFIX}{id}";
            await _cache.RemoveAsync(key);
            
            await InvalidateTenantsListCache();
            
            _logger.LogDebug("Removed tenant {Id} from cache", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove tenant {Id} from cache", id);
        }
    }

    public async Task RemoveTenantByTenantIdAsync(string tenantId)
    {
        try
        {
            var key = $"{TENANT_BY_TENANT_ID_PREFIX}{tenantId}";
            await _cache.RemoveAsync(key);
            
            await InvalidateTenantsListCache();
            
            _logger.LogDebug("Removed tenant by tenantId {TenantId} from cache", tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove tenant by tenantId {TenantId} from cache", tenantId);
        }
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
