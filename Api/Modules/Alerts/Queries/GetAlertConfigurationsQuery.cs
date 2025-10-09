using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Queries;

public record GetAlertConfigurationsQuery(
) : IRequest<GetAlertConfigurationsQuery, Task<List<AlertConfigurationDto>>>;