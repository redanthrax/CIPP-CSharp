using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class GetAlertConfigurationsQueryHandler : IRequestHandler<GetAlertConfigurationsQuery, Task<PagedResponse<AlertConfigurationDto>>> {
    private readonly IAlertConfigurationService _alertConfigurationService;
    private readonly ILogger<GetAlertConfigurationsQueryHandler> _logger;

    public GetAlertConfigurationsQueryHandler(
        IAlertConfigurationService alertConfigurationService,
        ILogger<GetAlertConfigurationsQueryHandler> logger) {
        _alertConfigurationService = alertConfigurationService;
        _logger = logger;
    }

    public async Task<PagedResponse<AlertConfigurationDto>> Handle(GetAlertConfigurationsQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving alert configurations");
            return await _alertConfigurationService.GetAlertConfigurationsAsync(request.Paging);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve alert configurations");
            return new PagedResponse<AlertConfigurationDto> {
                Items = new List<AlertConfigurationDto>(),
                TotalCount = 0,
                PageNumber = request.Paging?.PageNumber ?? 1,
                PageSize = request.Paging?.PageSize ?? 50
            };
        }
    }
}
