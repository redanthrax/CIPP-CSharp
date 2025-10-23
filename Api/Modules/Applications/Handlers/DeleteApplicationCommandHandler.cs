using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, Task> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<DeleteApplicationCommandHandler> _logger;

    public DeleteApplicationCommandHandler(
        IApplicationService applicationService,
        ILogger<DeleteApplicationCommandHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task Handle(DeleteApplicationCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Deleting application {ApplicationId} for tenant {TenantId}", request.ApplicationId, request.TenantId);
            await _applicationService.DeleteApplicationAsync(request.TenantId, request.ApplicationId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete application {ApplicationId}", request.ApplicationId);
            throw new InvalidOperationException($"Failed to delete application: {ex.Message}", ex);
        }
    }
}
