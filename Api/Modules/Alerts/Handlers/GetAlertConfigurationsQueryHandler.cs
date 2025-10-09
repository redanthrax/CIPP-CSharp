using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class GetAlertConfigurationsQueryHandler : IRequestHandler<GetAlertConfigurationsQuery, Task<List<AlertConfigurationDto>>> {
    private readonly IAlertConfigurationService _alertConfigurationService;
    private readonly ILogger<GetAlertConfigurationsQueryHandler> _logger;

    public GetAlertConfigurationsQueryHandler(
        IAlertConfigurationService alertConfigurationService,
        ILogger<GetAlertConfigurationsQueryHandler> logger) {
        _alertConfigurationService = alertConfigurationService;
        _logger = logger;
    }

    public async Task<List<AlertConfigurationDto>> Handle(GetAlertConfigurationsQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving alert configurations");
            return await _alertConfigurationService.GetAlertConfigurationsAsync();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve alert configurations");
            return new List<AlertConfigurationDto>();
        }
    }
}