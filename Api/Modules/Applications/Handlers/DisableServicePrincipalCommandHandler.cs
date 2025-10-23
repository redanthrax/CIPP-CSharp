using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DisableServicePrincipalCommandHandler : IRequestHandler<DisableServicePrincipalCommand, Task> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<DisableServicePrincipalCommandHandler> _logger;

    public DisableServicePrincipalCommandHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<DisableServicePrincipalCommandHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task Handle(DisableServicePrincipalCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Disabling service principal {ServicePrincipalId} for tenant {TenantId}", request.ServicePrincipalId, request.TenantId);
            await _servicePrincipalService.DisableServicePrincipalAsync(request.TenantId, request.ServicePrincipalId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to disable service principal {ServicePrincipalId}", request.ServicePrincipalId);
            throw new InvalidOperationException($"Failed to disable service principal: {ex.Message}", ex);
        }
    }
}
