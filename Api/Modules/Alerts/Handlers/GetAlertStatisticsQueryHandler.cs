using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class GetAlertStatisticsQueryHandler : IRequestHandler<GetAlertStatisticsQuery, Task<AlertStatisticsDto>> {
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAlertStatisticsQueryHandler> _logger;

    public GetAlertStatisticsQueryHandler(
        ApplicationDbContext dbContext,
        ILogger<GetAlertStatisticsQueryHandler> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<AlertStatisticsDto> Handle(
        GetAlertStatisticsQuery request,
        CancellationToken cancellationToken) {
        try {
            var now = DateTime.UtcNow;
            var startOfToday = now.Date;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var startDate = request.StartDate ?? startOfMonth;
            var endDate = request.EndDate ?? now;

            var auditLogs = await _dbContext.GetEntitySet<AuditLog>()
                .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
                .Include(a => a.WebhookRule)
                .ToListAsync(cancellationToken);

            var activeRules = await _dbContext.GetEntitySet<WebhookRule>()
                .CountAsync(r => r.IsActive, cancellationToken);

            var triggeredToday = auditLogs.Count(a => a.CreatedAt >= startOfToday);
            var triggeredThisWeek = auditLogs.Count(a => a.CreatedAt >= startOfWeek);
            var triggeredThisMonth = auditLogs.Count(a => a.CreatedAt >= startOfMonth);

            var alertsByType = auditLogs
                .GroupBy(a => a.WebhookRule?.LogType ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            var alertsByTenant = auditLogs
                .GroupBy(a => a.TenantFilter)
                .ToDictionary(g => g.Key, g => g.Count());

            var recentAlerts = auditLogs
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .Select(a => new RecentAlertDto {
                    Id = a.Id,
                    Title = a.Title,
                    TenantFilter = a.TenantFilter,
                    CreatedAt = a.CreatedAt
                })
                .ToList();

            return new AlertStatisticsDto {
                TotalAlerts = auditLogs.Count,
                ActiveRules = activeRules,
                TriggeredToday = triggeredToday,
                TriggeredThisWeek = triggeredThisWeek,
                TriggeredThisMonth = triggeredThisMonth,
                AlertsByType = alertsByType,
                AlertsByTenant = alertsByTenant,
                RecentAlerts = recentAlerts
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get alert statistics");
            return new AlertStatisticsDto();
        }
    }
}
