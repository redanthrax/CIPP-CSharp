using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Alerts.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class CreateScriptedAlertCommandHandler : IRequestHandler<CreateScriptedAlertCommand, Task<string>> {
    private readonly IAlertConfigurationService _alertConfigurationService;
    private readonly ILogger<CreateScriptedAlertCommandHandler> _logger;

    public CreateScriptedAlertCommandHandler(
        IAlertConfigurationService alertConfigurationService,
        ILogger<CreateScriptedAlertCommandHandler> logger) {
        _alertConfigurationService = alertConfigurationService;
        _logger = logger;
    }

    public async Task<string> Handle(CreateScriptedAlertCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Creating scripted alert");
            return await _alertConfigurationService.CreateScriptedAlertAsync(request.AlertData);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create scripted alert");
            throw new InvalidOperationException($"Failed to create scripted alert: {ex.Message}");
        }
    }
}