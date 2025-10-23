using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DeleteServicePrincipalCommandHandler : IRequestHandler<DeleteServicePrincipalCommand, Task> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<DeleteServicePrincipalCommandHandler> _logger;

    public DeleteServicePrincipalCommandHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<DeleteServicePrincipalCommandHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task Handle(DeleteServicePrincipalCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Deleting service principal {ServicePrincipalId} for tenant {TenantId}", request.ServicePrincipalId, request.TenantId);
            await _servicePrincipalService.DeleteServicePrincipalAsync(request.TenantId, request.ServicePrincipalId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete service principal {ServicePrincipalId}", request.ServicePrincipalId);
            throw new InvalidOperationException($"Failed to delete service principal: {ex.Message}", ex);
        }
    }
}
