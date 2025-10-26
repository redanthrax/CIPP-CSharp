using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Graph.Beta;
using System.Reflection;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace CIPP.Api.Modules.MsGraph.Services;

public class CachedGraphRequestHandler {
    private readonly IDistributedCache _cache;
    private readonly ILogger<CachedGraphRequestHandler> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);
    private const string CACHE_KEY_PREFIX = "graph:";

    public CachedGraphRequestHandler(IDistributedCache cache, ILogger<CachedGraphRequestHandler> logger) {
        _cache = cache;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<TResult?> ExecuteAsync<TResult>(
        Guid? tenantId,
        Func<Task<TResult?>> graphRequest,
        string resourcePath,
        string httpMethod = "GET",
        object? requestParameters = null) where TResult : class {
        
        var cacheKey = BuildCacheKey(tenantId ?? Guid.Empty, resourcePath, requestParameters);

        if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
            var cached = await GetFromCacheAsync<TResult>(cacheKey);
            if (cached != null) {
                _logger.LogDebug("Cache hit for {TenantId}:{ResourcePath}", tenantId, resourcePath);
                return cached;
            }
            _logger.LogDebug("Cache miss for {TenantId}:{ResourcePath}", tenantId, resourcePath);
        }

        try {
            var result = await graphRequest();

            if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase) && result != null) {
                await SetCacheAsync(cacheKey, result);
                _logger.LogDebug("Cached result for {TenantId}:{ResourcePath}", tenantId, resourcePath);
            } else if (!httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
                await InvalidateCacheForResourceAsync(tenantId ?? Guid.Empty, resourcePath);
                _logger.LogDebug("Invalidated cache for {TenantId}:{ResourcePath} after {Method}", tenantId, resourcePath, httpMethod);
            }

            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Graph request failed for {TenantId}:{ResourcePath}", tenantId, resourcePath);
            throw;
        }
    }

    public async Task ExecuteAsync(
        Guid? tenantId,
        Func<Task> graphRequest,
        string resourcePath,
        string httpMethod) {
        
        try {
            await graphRequest();

            if (!httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
                await InvalidateCacheForResourceAsync(tenantId ?? Guid.Empty, resourcePath);
                _logger.LogDebug("Invalidated cache for {TenantId}:{ResourcePath} after {Method}", tenantId, resourcePath, httpMethod);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Graph request failed for {TenantId}:{ResourcePath}", tenantId, resourcePath);
            throw;
        }
    }

    private string BuildCacheKey(Guid tenantId, string resourcePath, object? parameters) {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append(CACHE_KEY_PREFIX);
        keyBuilder.Append(tenantId);
        keyBuilder.Append(':');
        keyBuilder.Append(NormalizeResourcePath(resourcePath));

        if (parameters != null) {
            var paramString = SerializeParameters(parameters);
            if (!string.IsNullOrEmpty(paramString)) {
                var hash = ComputeHash(paramString);
                keyBuilder.Append(':');
                keyBuilder.Append(hash);
            }
        }

        return keyBuilder.ToString();
    }

    private string NormalizeResourcePath(string resourcePath) {
        var path = resourcePath.Replace("https://graph.microsoft.com/beta/", "")
                               .Replace("https://graph.microsoft.com/v1.0/", "")
                               .Trim('/');

        var parts = path.Split('/');
        var normalized = new StringBuilder();

        for (int i = 0; i < parts.Length; i++) {
            if (i > 0) normalized.Append(':');

            if (Guid.TryParse(parts[i], out _) || 
                parts[i].Contains('@') || 
                (i > 0 && IsIdSegment(parts[i - 1]))) {
                normalized.Append("*");
            } else {
                normalized.Append(parts[i]);
            }
        }

        return normalized.ToString();
    }

    private bool IsIdSegment(string segment) {
        return segment.Equals("users", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("groups", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("devices", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("applications", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("servicePrincipals", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("members", StringComparison.OrdinalIgnoreCase) ||
               segment.Equals("owners", StringComparison.OrdinalIgnoreCase);
    }

    private string SerializeParameters(object parameters) {
        try {
            var properties = parameters.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var paramDict = new Dictionary<string, object?>();

            foreach (var prop in properties) {
                var value = prop.GetValue(parameters);
                if (value != null) {
                    paramDict[prop.Name] = value;
                }
            }

            return JsonSerializer.Serialize(paramDict, _jsonOptions);
        } catch {
            return string.Empty;
        }
    }

    private string ComputeHash(string input) {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash)[..16].Replace("+", "").Replace("/", "").Replace("=", "");
    }

    private async Task<T?> GetFromCacheAsync<T>(string key) where T : class {
        try {
            var json = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(json)) {
                return null;
            }

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get cached value for key: {Key}", key);
            return null;
        }
    }

    private async Task SetCacheAsync<T>(string key, T value) where T : class {
        try {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = CacheExpiration
            };

            await _cache.SetStringAsync(key, json, options);
            await AddKeyToTrackingAsync(key);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to cache value for key: {Key}", key);
        }
    }

    private async Task InvalidateCacheForResourceAsync(Guid tenantId, string resourcePath) {
        try {
            var normalizedPath = NormalizeResourcePath(resourcePath);
            var parts = normalizedPath.Split(':');

            if (parts.Length > 0) {
                var baseResource = parts[0];
                var pattern = $"{CACHE_KEY_PREFIX}{tenantId}:{baseResource}";
                await RemoveCacheByPatternAsync(pattern);
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to invalidate cache for resource: {ResourcePath}", resourcePath);
        }
    }

    private async Task RemoveCacheByPatternAsync(string pattern) {
        try {
            var keysSetKey = $"{CACHE_KEY_PREFIX}keys";
            var keysJson = await _cache.GetStringAsync(keysSetKey);
            
            if (string.IsNullOrEmpty(keysJson)) {
                return;
            }

            var allKeys = JsonSerializer.Deserialize<HashSet<string>>(keysJson, _jsonOptions);
            if (allKeys == null) {
                return;
            }

            var matchingKeys = allKeys.Where(k => k.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)).ToList();
            
            foreach (var key in matchingKeys) {
                await _cache.RemoveAsync(key);
                allKeys.Remove(key);
            }

            if (matchingKeys.Count > 0) {
                var updatedJson = JsonSerializer.Serialize(allKeys, _jsonOptions);
                await _cache.SetStringAsync(keysSetKey, updatedJson);
                _logger.LogDebug("Removed {Count} cache entries matching pattern: {Pattern}", matchingKeys.Count, pattern);
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to remove cache entries by pattern: {Pattern}", pattern);
        }
    }

    private async Task AddKeyToTrackingAsync(string key) {
        try {
            var keysSetKey = $"{CACHE_KEY_PREFIX}keys";
            var keysJson = await _cache.GetStringAsync(keysSetKey);
            var keys = string.IsNullOrEmpty(keysJson)
                ? new HashSet<string>()
                : JsonSerializer.Deserialize<HashSet<string>>(keysJson, _jsonOptions) ?? new HashSet<string>();

            if (keys.Add(key)) {
                var updatedJson = JsonSerializer.Serialize(keys, _jsonOptions);
                var options = new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                await _cache.SetStringAsync(keysSetKey, updatedJson, options);
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to add key to tracking: {Key}", key);
        }
    }
}
