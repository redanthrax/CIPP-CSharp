using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Queries;

public record GetAlertStatisticsQuery(
    DateTime? StartDate,
    DateTime? EndDate) : IRequest<GetAlertStatisticsQuery, Task<AlertStatisticsDto>>;
