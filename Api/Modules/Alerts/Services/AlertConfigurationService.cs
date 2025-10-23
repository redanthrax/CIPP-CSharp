using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using CIPP.Shared.DTOs.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertConfigurationService : IAlertConfigurationService {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMicrosoftGraphService _graphService;
    private readonly IAlertCacheService _cacheService;
    private readonly IAlertJobService _alertJobService;
    private readonly ILogger<AlertConfigurationService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AlertConfigurationService(
        ApplicationDbContext dbContext,
        IMicrosoftGraphService graphService,
        IAlertCacheService cacheService,
        IAlertJobService alertJobService,
        ILogger<AlertConfigurationService> logger) {
        _dbContext = dbContext;
        _graphService = graphService;
        _cacheService = cacheService;
        _alertJobService = alertJobService;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<PagedResponse<AlertConfigurationDto>> GetAlertConfigurationsAsync(PagingParameters? paging = null) {
        try {
            paging ??= new PagingParameters();
            
            var cachedAlerts = await _cacheService.GetAlertConfigurationsAsync();
            if (cachedAlerts.Any()) {
                var pagedCached = cachedAlerts
                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                    .Take(paging.PageSize)
                    .ToList();
                    
                return new PagedResponse<AlertConfigurationDto> {
                    Items = pagedCached,
                    TotalCount = cachedAlerts.Count,
                    PageNumber = paging.PageNumber,
                    PageSize = paging.PageSize
                };
            }

            var query = _dbContext.GetEntitySet<AlertConfiguration>()
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt);
                
            var totalCount = await query.CountAsync();
            
            var alertConfigs = await query
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

            var alerts = new List<AlertConfigurationDto>();

            foreach (var config in alertConfigs) {
                var alert = new AlertConfigurationDto {
                    RowKey = config.Id.ToString(),
                    PartitionKey = config.AlertType,
                    EventType = config.AlertType + " Alert",
                    LogType = config.LogType,
                    Conditions = config.Conditions,
                    Actions = config.Actions,
                    RepeatsEvery = config.ScheduleCron ?? "On demand",
                    Tenants = JsonSerializer.Deserialize<List<TenantSelectorOptionDto>>(config.TenantFilter, _jsonOptions) ?? new List<TenantSelectorOptionDto>(),
                    ExcludedTenants = JsonSerializer.Deserialize<List<string>>(config.ExcludedTenants, _jsonOptions) ?? new List<string>(),
                    RawAlert = new {
                        Id = config.Id,
                        Name = config.Name,
                        CreatedAt = config.CreatedAt,
                        LastExecuted = config.LastExecuted,
                        LastExecutionResult = config.LastExecutionResult,
                        HangfireJobId = config.HangfireJobId,
                        WebhookSubscriptionId = config.WebhookSubscriptionId
                    }
                };
                alerts.Add(alert);
            }

            return new PagedResponse<AlertConfigurationDto> {
                Items = alerts,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get alert configurations");
            return new PagedResponse<AlertConfigurationDto> {
                Items = new List<AlertConfigurationDto>(),
                TotalCount = 0,
                PageNumber = paging?.PageNumber ?? 1,
                PageSize = paging?.PageSize ?? 50
            };
        }
    }

    public async Task<string> CreateAuditLogAlertAsync(CreateAuditLogAlertDto alertData) {
        try {
            var alertConfig = new AlertConfiguration {
                Id = Guid.NewGuid(),
                Name = $"Audit Log Alert - {alertData.Logbook?.Label ?? "Unknown"}",
                AlertType = "AuditLog",
                LogType = alertData.Logbook?.Value ?? "Unknown",
                TenantFilter = JsonSerializer.Serialize(alertData.TenantFilter, _jsonOptions),
                ExcludedTenants = JsonSerializer.Serialize(alertData.ExcludedTenants.Select(t => t.Value).ToList(), _jsonOptions),
                Conditions = JsonSerializer.Serialize(alertData.Conditions, _jsonOptions),
                Actions = JsonSerializer.Serialize(alertData.Actions, _jsonOptions)
            };

            string? webhookSubscriptionId = null;
            try {
                var graphClient = await _graphService.GetGraphServiceClientAsync();
                var subscription = new Subscription {
                    Resource = GetResourceFromLogbook(alertData.Logbook?.Value),
                    ChangeType = "created,updated",
                    NotificationUrl = "https://your-webhook-endpoint.com/webhook", // This should be configurable
                    ExpirationDateTime = DateTime.UtcNow.AddDays(3),
                    ClientState = Guid.NewGuid().ToString()
                };

                var createdSubscription = await graphClient.Subscriptions.PostAsync(subscription);
                webhookSubscriptionId = createdSubscription?.Id;
                alertConfig.WebhookSubscriptionId = webhookSubscriptionId;
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Failed to create webhook subscription, continuing without it");
            }

            _dbContext.GetEntitySet<AlertConfiguration>().Add(alertConfig);
            await _dbContext.SaveChangesAsync();

            var jobId = $"audit-alert-{alertConfig.Id}";
            alertConfig.HangfireJobId = jobId;
            await _alertJobService.ScheduleRecurringAlertAsync(jobId, alertConfig.Id, "0 0 */2 * *"); 

            await _dbContext.SaveChangesAsync();

            await _cacheService.InvalidateAlertConfigurationsCache();

            var result = $"Created audit log alert '{alertConfig.Name}' with ID: {alertConfig.Id}";
            if (webhookSubscriptionId != null) {
                result += $" and webhook subscription: {webhookSubscriptionId}";
            }
            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create audit log alert");
            throw new InvalidOperationException($"Failed to create audit log alert: {ex.Message}");
        }
    }

    public async Task<string> RemoveAlertAsync(string id, string eventType) {
        try {
            if (!Guid.TryParse(id, out var alertId)) {
                throw new ArgumentException($"Invalid alert ID format: {id}");
            }

            var alertConfig = await _dbContext.GetEntitySet<AlertConfiguration>()
                .FirstOrDefaultAsync(a => a.Id == alertId);

            if (alertConfig == null) {
                throw new InvalidOperationException($"Alert configuration {id} not found");
            }

            if (!string.IsNullOrEmpty(alertConfig.HangfireJobId)) {
                await _alertJobService.RemoveJobAsync(alertConfig.HangfireJobId);
            }

            if (!string.IsNullOrEmpty(alertConfig.WebhookSubscriptionId)) {
                try {
                    var graphClient = await _graphService.GetGraphServiceClientAsync();
                    await graphClient.Subscriptions[alertConfig.WebhookSubscriptionId].DeleteAsync();
                } catch (Exception ex) {
                    _logger.LogWarning(ex, "Failed to remove webhook subscription {SubscriptionId}, continuing with alert removal", 
                        alertConfig.WebhookSubscriptionId);
                }
            }

            alertConfig.IsActive = false;
            await _dbContext.SaveChangesAsync();

            await _cacheService.InvalidateAlertConfigurationsCache();

            return $"Successfully removed alert '{alertConfig.Name}' ({alertId})";
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove alert {Id}", id);
            throw new InvalidOperationException($"Failed to remove alert: {ex.Message}");
        }
    }

    public async Task<string> CreateScriptedAlertAsync(CreateScriptedAlertDto alertData) {
        try {
            var alertConfig = new AlertConfiguration {
                Id = Guid.NewGuid(),
                Name = alertData.Name ?? $"Scripted Alert - {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
                AlertType = "Scripted",
                LogType = alertData.LogType ?? "General",
                TenantFilter = JsonSerializer.Serialize(alertData.TenantFilter, _jsonOptions),
                ExcludedTenants = JsonSerializer.Serialize(alertData.ExcludedTenants?.Select(t => t.Value).ToList() ?? new List<string>(), _jsonOptions),
                Conditions = JsonSerializer.Serialize(alertData.Conditions, _jsonOptions),
                Actions = JsonSerializer.Serialize(alertData.Actions, _jsonOptions),
                ScheduleCron = alertData.ScheduleCron ?? "0 0 * * *",
                RawConfiguration = JsonSerializer.Serialize(alertData, _jsonOptions)
            };

            _dbContext.GetEntitySet<AlertConfiguration>().Add(alertConfig);
            await _dbContext.SaveChangesAsync();

            var jobId = $"scripted-alert-{alertConfig.Id}";
            alertConfig.HangfireJobId = jobId;
            await _alertJobService.ScheduleRecurringAlertAsync(jobId, alertConfig.Id, alertConfig.ScheduleCron);

            await _dbContext.SaveChangesAsync();

            await _cacheService.InvalidateAlertConfigurationsCache();

            return $"Created scripted alert '{alertConfig.Name}' with ID: {alertConfig.Id} (scheduled: {alertConfig.ScheduleCron})";
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create scripted alert");
            throw new InvalidOperationException($"Failed to create scripted alert: {ex.Message}");
        }
    }

    private static string GetLogTypeFromResource(string? resource) {
        return resource switch {
            var r when r?.Contains("auditLogs/directoryAudits") == true => "Audit.AzureActiveDirectory",
            var r when r?.Contains("auditLogs/signIns") == true => "Audit.SignIns",
            _ => "Unknown"
        };
    }

    private static string GetResourceFromLogbook(string? logbookValue) {
        return logbookValue switch {
            "Audit.AzureActiveDirectory" => "auditLogs/directoryAudits",
            "Audit.Exchange" => "auditLogs/directoryAudits",
            _ => "auditLogs/directoryAudits"
        };
    }
}
