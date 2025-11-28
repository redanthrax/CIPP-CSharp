using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Queries;

public record GetAlertHistoryQuery(
    string? TenantFilter,
    DateTime? StartDate,
    DateTime? EndDate,
    string? LogType,
    PagingParameters? Paging) : IRequest<GetAlertHistoryQuery, Task<PagedResponse<AlertHistoryDto>>>;
