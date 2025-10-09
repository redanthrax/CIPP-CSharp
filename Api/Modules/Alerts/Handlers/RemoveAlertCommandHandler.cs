using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Alerts.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class RemoveAlertCommandHandler : IRequestHandler<RemoveAlertCommand, Task<string>> {
    private readonly IAlertConfigurationService _alertConfigurationService;
    private readonly ILogger<RemoveAlertCommandHandler> _logger;

    public RemoveAlertCommandHandler(
        IAlertConfigurationService alertConfigurationService,
        ILogger<RemoveAlertCommandHandler> logger) {
        _alertConfigurationService = alertConfigurationService;
        _logger = logger;
    }

    public async Task<string> Handle(RemoveAlertCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Removing alert {Id}", request.Id);
            return await _alertConfigurationService.RemoveAlertAsync(request.Id, request.EventType);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove alert {Id}", request.Id);
            throw new InvalidOperationException($"Failed to remove alert: {ex.Message}");
        }
    }
}