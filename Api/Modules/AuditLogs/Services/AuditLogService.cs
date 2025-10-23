using CIPP.Api.Data;
using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.AuditLogs.Models;
using CIPP.Shared.DTOs.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CIPP.Api.Modules.AuditLogs.Services;

public class AuditLogService : IAuditLogService {
    private readonly ILogger<AuditLogService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditLogService(
        ILogger<AuditLogService> logger,
        ApplicationDbContext context,
        IDistributedCache cache) {
        _logger = logger;
        _context = context;
        _cache = cache;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<AuditLogResponseDto> GetAuditLogsAsync(
        string? tenantFilter = null,
        string? logId = null,
        string? startDate = null,
        string? endDate = null,
        string? relativeTime = null,
        CancellationToken cancellationToken = default) {
        
        try {
            var cacheKey = GenerateCacheKey(tenantFilter, logId, startDate, endDate, relativeTime);
            var cachedResult = await GetCachedResultAsync(cacheKey, cancellationToken);
            
            if (cachedResult != null) {
                _logger.LogDebug("Returning cached audit logs for key: {CacheKey}", cacheKey);
                return cachedResult;
            }

            var query = BuildQuery(tenantFilter, logId, startDate, endDate, relativeTime);
            var auditLogs = await ExecuteQueryAsync(query, cancellationToken);
            
            var result = new AuditLogResponseDto(
                Results: auditLogs,
                Metadata: new AuditLogMetadataDto(
                    Count: auditLogs.Count,
                    Filter: BuildFilterDescription(tenantFilter, logId, startDate, endDate, relativeTime)
                )
            );

            await CacheResultAsync(cacheKey, result, cancellationToken);
            
            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving audit logs");
            throw;
        }
    }

    private IQueryable<AuditLog> BuildQuery(
        string? tenantFilter,
        string? logId,
        string? startDate,
        string? endDate,
        string? relativeTime) {
        
        var query = _context.GetEntitySet<AuditLog>().AsQueryable();

        if (!string.IsNullOrEmpty(logId)) {
            query = query.Where(x => x.LogId == logId);
            return query;
        }

        if (!string.IsNullOrEmpty(tenantFilter) && tenantFilter != "AllTenants") {
            query = query.Where(x => x.Tenant == tenantFilter);
        }

        var (startDateTime, endDateTime) = ProcessDateFilters(startDate, endDate, relativeTime);
        query = query.Where(x => x.Timestamp >= startDateTime && x.Timestamp <= endDateTime);

        return query;
    }

    private (DateTime startDateTime, DateTime endDateTime) ProcessDateFilters(
        string? startDate, string? endDate, string? relativeTime) {
        
        if (!string.IsNullOrEmpty(relativeTime)) {
            return ProcessRelativeTime(relativeTime);
        }

        return ProcessAbsoluteDateRange(startDate, endDate);
    }

    private (DateTime startDateTime, DateTime endDateTime) ProcessRelativeTime(string relativeTime) {
        var endDateTime = DateTime.UtcNow;
        var startDateTime = endDateTime;

        var match = Regex.Match(relativeTime, @"(\d+)([dhm])");
        if (match.Success) {
            var interval = int.Parse(match.Groups[1].Value);
            var unit = match.Groups[2].Value;

            startDateTime = unit switch {
                "d" => endDateTime.AddDays(-interval),
                "h" => endDateTime.AddHours(-interval),
                "m" => endDateTime.AddMinutes(-interval),
                _ => endDateTime.AddDays(-7)
            };
        } else {
            startDateTime = endDateTime.AddDays(-7);
        }

        return (startDateTime, endDateTime);
    }

    private (DateTime startDateTime, DateTime endDateTime) ProcessAbsoluteDateRange(
        string? startDate, string? endDate) {
        
        var endDateTime = DateTime.UtcNow;
        var startDateTime = endDateTime.AddDays(-7);

        if (!string.IsNullOrEmpty(startDate)) {
            startDateTime = ParseDateParameter(startDate);
        }

        if (!string.IsNullOrEmpty(endDate)) {
            endDateTime = ParseDateParameter(endDate);
        }

        return (startDateTime, endDateTime);
    }

    private DateTime ParseDateParameter(string dateParam) {
        if (long.TryParse(dateParam, out var unixTimestamp)) {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime.ToUniversalTime();
        }

        if (DateTime.TryParse(dateParam, out var dateTime)) {
            return dateTime.ToUniversalTime();
        }

        throw new ArgumentException($"Invalid date format: {dateParam}");
    }

    private async Task<List<AuditLogDto>> ExecuteQueryAsync(
        IQueryable<AuditLog> query, 
        CancellationToken cancellationToken) {
        
        var entities = await query
            .OrderByDescending(x => x.Timestamp)
            .Take(1000) // Limit results for performance
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Retrieved {Count} audit log entities from database", entities.Count);

        return entities.Select(ConvertToDto).ToList();
    }

    private AuditLogDto ConvertToDto(AuditLog entity) {
        var rawData = new AuditLogRawDataDto(
            CreationTime: entity.CreationTime,
            CIPPUserKey: entity.CIPPUserKey,
            UserId: entity.UserId,
            UserKey: entity.UserKey,
            ClientIP: entity.ClientIP,
            CIPPAction: entity.CIPPAction,
            CIPPClause: entity.CIPPClause,
            Operation: entity.Operation,
            Workload: entity.Workload,
            ResultStatus: entity.ResultStatus,
            ObjectId: entity.ObjectId,
            RecordType: entity.RecordType,
            OrganizationId: entity.OrganizationId,
            UserType: entity.UserType,
            AdditionalProperties: entity.AdditionalProperties
        );

        var data = new AuditLogDataDto(
            IP: entity.IP,
            RawData: rawData,
            ActionUrl: entity.ActionUrl,
            ActionText: entity.ActionText
        );

        return new AuditLogDto(
            LogId: entity.LogId,
            Timestamp: entity.Timestamp,
            Tenant: entity.Tenant,
            Title: entity.Title,
            Data: data
        );
    }

    private string GenerateCacheKey(
        string? tenantFilter,
        string? logId,
        string? startDate,
        string? endDate,
        string? relativeTime) {
        
        var keyParts = new List<string> { "audit-logs" };
        
        if (!string.IsNullOrEmpty(tenantFilter)) keyParts.Add($"tenant:{tenantFilter}");
        if (!string.IsNullOrEmpty(logId)) keyParts.Add($"log:{logId}");
        if (!string.IsNullOrEmpty(startDate)) keyParts.Add($"start:{startDate}");
        if (!string.IsNullOrEmpty(endDate)) keyParts.Add($"end:{endDate}");
        if (!string.IsNullOrEmpty(relativeTime)) keyParts.Add($"relative:{relativeTime}");
        
        return string.Join(":", keyParts);
    }

    private async Task<AuditLogResponseDto?> GetCachedResultAsync(
        string cacheKey, 
        CancellationToken cancellationToken) {
        
        try {
            var cachedJson = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedJson)) {
                return JsonSerializer.Deserialize<AuditLogResponseDto>(cachedJson, _jsonOptions);
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to retrieve cached audit logs for key: {CacheKey}", cacheKey);
        }
        
        return null;
    }

    private async Task CacheResultAsync(
        string cacheKey, 
        AuditLogResponseDto result, 
        CancellationToken cancellationToken) {
        
        try {
            var cacheOptions = new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            var json = JsonSerializer.Serialize(result, _jsonOptions);
            await _cache.SetStringAsync(cacheKey, json, cacheOptions, cancellationToken);
            
            _logger.LogDebug("Cached audit logs result for key: {CacheKey}", cacheKey);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to cache audit logs for key: {CacheKey}", cacheKey);
        }
    }

    private string BuildFilterDescription(
        string? tenantFilter,
        string? logId,
        string? startDate,
        string? endDate,
        string? relativeTime) {
        
        var filters = new List<string>();
        
        if (!string.IsNullOrEmpty(logId)) {
            filters.Add($"LogId eq '{logId}'");
        } else {
            if (!string.IsNullOrEmpty(tenantFilter) && tenantFilter != "AllTenants") {
                filters.Add($"Tenant eq '{tenantFilter}'");
            }
            
            if (!string.IsNullOrEmpty(relativeTime)) {
                filters.Add($"RelativeTime: {relativeTime}");
            } else {
                if (!string.IsNullOrEmpty(startDate)) {
                    filters.Add($"StartDate >= {startDate}");
                }
                if (!string.IsNullOrEmpty(endDate)) {
                    filters.Add($"EndDate <= {endDate}");
                }
            }
        }
        
        return string.Join(" and ", filters);
    }
}