using CIPP.Api.Data;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.AuditLogs.Models;
using CIPP.Shared.DTOs.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CIPP.Api.Modules.AuditLogs.Services;

public class AuditLogSearchService : IAuditLogSearchService {
    private readonly ILogger<AuditLogSearchService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IMicrosoftGraphService _graphService;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditLogSearchService(
        ILogger<AuditLogSearchService> logger,
        ApplicationDbContext context,
        IDistributedCache cache,
        IMicrosoftGraphService graphService) {
        _logger = logger;
        _context = context;
        _cache = cache;
        _graphService = graphService;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<AuditLogSearchListResponseDto> GetSearchesAsync(
        string tenantFilter,
        int? days = null,
        CancellationToken cancellationToken = default) {
        
        try {
            var effectiveDays = days ?? 1;
            var startTime = DateTime.UtcNow.AddDays(-effectiveDays);

            _logger.LogInformation("Getting audit log searches for tenant {Tenant} since {StartTime}", 
                tenantFilter, startTime);

            var query = _context.GetEntitySet<AuditLogSearch>()
                .Where(s => s.Tenant == tenantFilter && s.CreatedAt >= startTime)
                .OrderByDescending(s => s.CreatedAt);

            var searches = await query.ToListAsync(cancellationToken);

            var searchDtos = searches.Select(ConvertToSearchDto).ToList();

            return new AuditLogSearchListResponseDto(
                Results: searchDtos,
                Metadata: new AuditLogSearchListMetadataDto(
                    TenantFilter: tenantFilter,
                    TotalSearches: searchDtos.Count,
                    StartTime: startTime
                )
            );
        } catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving audit log searches for tenant {Tenant}", tenantFilter);
            throw;
        }
    }

    public async Task<AuditLogSearchResultDto> GetSearchResultsAsync(
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default) {
        
        try {
            _logger.LogInformation("Getting search results for search {SearchId} in tenant {Tenant}", 
                searchId, tenantFilter);

            var search = await _context.GetEntitySet<AuditLogSearch>()
                .FirstOrDefaultAsync(s => s.SearchId == searchId && s.Tenant == tenantFilter, cancellationToken);

            if (search == null) {
                throw new InvalidOperationException($"Search with ID {searchId} not found for tenant {tenantFilter}");
            }

            var auditLogs = await GetAuditLogsForSearchAsync(search, cancellationToken);

            return new AuditLogSearchResultDto(
                Results: auditLogs,
                Metadata: new AuditLogSearchMetadataDto(
                    SearchId: searchId,
                    TenantFilter: tenantFilter,
                    TotalResults: auditLogs.Count,
                    Status: search.Status,
                    StartTime: search.StartTime,
                    EndTime: search.EndTime,
                    ErrorMessage: search.ErrorMessage
                )
            );
        } catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving search results for search {SearchId}", searchId);
            throw;
        }
    }

    public async Task<AuditLogSearchDto> CreateSearchAsync(
        CreateAuditLogSearchDto request,
        CancellationToken cancellationToken = default) {
        
        try {
            _logger.LogInformation("Creating audit log search for tenant {Tenant}", request.TenantFilter);

            var searchId = Guid.NewGuid().ToString();
            var displayName = request.DisplayName ?? $"Search-{DateTime.UtcNow:yyyyMMdd-HHmmss}";

            var query = BuildSearchQuery(request);

            var searchEntity = new AuditLogSearch {
                Id = Guid.NewGuid(),
                SearchId = searchId,
                Tenant = request.TenantFilter,
                DisplayName = displayName,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = "Created",
                CreatedAt = DateTime.UtcNow,
                Query = query
            };

            _context.Add(searchEntity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created audit log search {SearchId} for tenant {Tenant}", 
                searchId, request.TenantFilter);

            return ConvertToSearchDto(searchEntity);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating audit log search for tenant {Tenant}", request.TenantFilter);
            throw;
        }
    }

    public async Task<AuditLogSearchDto> ProcessSearchAsync(
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default) {
        
        try {
            _logger.LogInformation("Processing audit log search {SearchId} for tenant {Tenant}", 
                searchId, tenantFilter);

            var search = await _context.GetEntitySet<AuditLogSearch>()
                .FirstOrDefaultAsync(s => s.SearchId == searchId && s.Tenant == tenantFilter, cancellationToken);

            if (search == null) {
                throw new InvalidOperationException($"Search with ID {searchId} not found for tenant {tenantFilter}");
            }

            search.Status = "Processing";
            search.ProcessedAt = DateTime.UtcNow;

            try {
                var (totalLogs, matchedLogs) = await ProcessSearchWithGraphAsync(search, cancellationToken);
                
                search.TotalLogs = totalLogs;
                search.MatchedLogs = matchedLogs;
                search.Status = "Completed";
                
                _logger.LogInformation("Completed processing search {SearchId}: {MatchedLogs}/{TotalLogs} logs matched", 
                    searchId, matchedLogs, totalLogs);
            } catch (Exception ex) {
                search.Status = "Failed";
                search.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Failed to process search {SearchId}", searchId);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ConvertToSearchDto(search);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error processing audit log search {SearchId}", searchId);
            throw;
        }
    }

    private async Task<List<AuditLogDto>> GetAuditLogsForSearchAsync(
        AuditLogSearch search, 
        CancellationToken cancellationToken) {
        
        var query = _context.GetEntitySet<AuditLog>()
            .Where(log => log.Tenant == search.Tenant && 
                         log.Timestamp >= search.StartTime && 
                         log.Timestamp <= search.EndTime);

        if (search.Query != null) {
            query = ApplySearchFilters(query, search.Query);
        }

        var logs = await query
            .OrderByDescending(log => log.Timestamp)
            .Take(1000)
            .ToListAsync(cancellationToken);

        return logs.Select(ConvertAuditLogToDto).ToList();
    }

    private IQueryable<AuditLog> ApplySearchFilters(IQueryable<AuditLog> query, Dictionary<string, object> filters) {
        if (filters.TryGetValue("Operations", out var operations) && operations is JsonElement operationsElement) {
            var operationsList = operationsElement.EnumerateArray()
                .Select(e => e.GetString())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
            
            if (operationsList.Any()) {
                query = query.Where(log => operationsList.Contains(log.Operation));
            }
        }

        if (filters.TryGetValue("UserIds", out var userIds) && userIds is JsonElement userIdsElement) {
            var userIdsList = userIdsElement.EnumerateArray()
                .Select(e => e.GetString())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
            
            if (userIdsList.Any()) {
                query = query.Where(log => userIdsList.Contains(log.UserId));
            }
        }

        if (filters.TryGetValue("RecordTypes", out var recordTypes) && recordTypes is JsonElement recordTypesElement) {
            var recordTypesList = recordTypesElement.EnumerateArray()
                .Select(e => e.GetInt32())
                .ToList();
            
            if (recordTypesList.Any()) {
                query = query.Where(log => log.RecordType.HasValue && recordTypesList.Contains(log.RecordType.Value));
            }
        }

        return query;
    }

    private async Task<(int totalLogs, int matchedLogs)> ProcessSearchWithGraphAsync(
        AuditLogSearch search, 
        CancellationToken cancellationToken) {
        
        var query = _context.GetEntitySet<AuditLog>()
            .Where(log => log.Tenant == search.Tenant && 
                         log.Timestamp >= search.StartTime && 
                         log.Timestamp <= search.EndTime);

        var totalLogs = await query.CountAsync(cancellationToken);

        if (search.Query != null) {
            query = ApplySearchFilters(query, search.Query);
        }

        var matchedLogs = await query.CountAsync(cancellationToken);

        return (totalLogs, matchedLogs);
    }

    private Dictionary<string, object> BuildSearchQuery(CreateAuditLogSearchDto request) {
        var query = new Dictionary<string, object>();

        if (request.Operations?.Any() == true) {
            query["Operations"] = request.Operations;
        }

        if (request.UserIds?.Any() == true) {
            query["UserIds"] = request.UserIds;
        }

        if (request.RecordTypes?.Any() == true) {
            query["RecordTypes"] = request.RecordTypes;
        }

        if (request.AdditionalFilters != null) {
            foreach (var filter in request.AdditionalFilters) {
                query[filter.Key] = filter.Value;
            }
        }

        return query;
    }

    private AuditLogSearchDto ConvertToSearchDto(AuditLogSearch entity) {
        return new AuditLogSearchDto(
            SearchId: entity.SearchId,
            StartTime: entity.StartTime,
            EndTime: entity.EndTime,
            DisplayName: entity.DisplayName,
            Tenant: entity.Tenant,
            Status: entity.Status,
            Query: entity.Query,
            MatchedRules: entity.MatchedRules,
            TotalLogs: entity.TotalLogs,
            MatchedLogs: entity.MatchedLogs,
            CreatedAt: entity.CreatedAt,
            ProcessedAt: entity.ProcessedAt,
            ErrorMessage: entity.ErrorMessage
        );
    }

    private AuditLogDto ConvertAuditLogToDto(AuditLog entity) {
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
}