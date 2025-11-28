using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Models;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class GetAlertHistoryQueryHandler : IRequestHandler<GetAlertHistoryQuery, Task<PagedResponse<AlertHistoryDto>>> {
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAlertHistoryQueryHandler> _logger;

    public GetAlertHistoryQueryHandler(
        ApplicationDbContext dbContext,
        ILogger<GetAlertHistoryQueryHandler> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<AlertHistoryDto>> Handle(
        GetAlertHistoryQuery request,
        CancellationToken cancellationToken) {
        try {
            var paging = request.Paging ?? new PagingParameters();

            var auditQuery = _dbContext.GetEntitySet<AuditLog>()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.TenantFilter)) {
                auditQuery = auditQuery.Where(a => a.TenantFilter == request.TenantFilter);
            }

            if (!string.IsNullOrEmpty(request.LogType)) {
                auditQuery = auditQuery.Where(a => a.Title.Contains(request.LogType));
            }

            if (request.StartDate.HasValue) {
                auditQuery = auditQuery.Where(a => a.CreatedAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue) {
                auditQuery = auditQuery.Where(a => a.CreatedAt <= request.EndDate.Value);
            }

            var totalCount = await auditQuery.CountAsync(cancellationToken);

            var auditLogs = await auditQuery
                .OrderByDescending(a => a.CreatedAt)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .Include(a => a.WebhookRule)
                .ToListAsync(cancellationToken);

            var history = auditLogs.Select(a => new AlertHistoryDto {
                Id = a.Id,
                TenantFilter = a.TenantFilter,
                Title = a.Title,
                LogType = a.WebhookRule?.LogType ?? "Unknown",
                IpAddress = a.IpAddress,
                RawData = a.RawData,
                CreatedAt = a.CreatedAt,
                WebhookRuleId = a.WebhookRuleId,
                RuleName = a.WebhookRule?.LogType
            }).ToList();

            return new PagedResponse<AlertHistoryDto> {
                Items = history,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get alert history");
            return new PagedResponse<AlertHistoryDto> {
                Items = new List<AlertHistoryDto>(),
                TotalCount = 0,
                PageNumber = request.Paging?.PageNumber ?? 1,
                PageSize = request.Paging?.PageSize ?? 50
            };
        }
    }
}
