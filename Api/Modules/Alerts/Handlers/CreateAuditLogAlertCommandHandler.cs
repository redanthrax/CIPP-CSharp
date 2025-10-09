using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Alerts.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Handlers;

public class CreateAuditLogAlertCommandHandler : IRequestHandler<CreateAuditLogAlertCommand, Task<string>> {
    private readonly IAlertConfigurationService _alertConfigurationService;
    private readonly ILogger<CreateAuditLogAlertCommandHandler> _logger;

    public CreateAuditLogAlertCommandHandler(
        IAlertConfigurationService alertConfigurationService,
        ILogger<CreateAuditLogAlertCommandHandler> logger) {
        _alertConfigurationService = alertConfigurationService;
        _logger = logger;
    }

    public async Task<string> Handle(CreateAuditLogAlertCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Creating audit log alert");
            return await _alertConfigurationService.CreateAuditLogAlertAsync(request.AlertData);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create audit log alert");
            throw new InvalidOperationException($"Failed to create audit log alert: {ex.Message}");
        }
    }
}