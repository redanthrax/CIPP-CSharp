using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class EnableServicePrincipalCommandHandler : IRequestHandler<EnableServicePrincipalCommand, Task> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<EnableServicePrincipalCommandHandler> _logger;

    public EnableServicePrincipalCommandHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<EnableServicePrincipalCommandHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task Handle(EnableServicePrincipalCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Enabling service principal {ServicePrincipalId} for tenant {TenantId}", request.ServicePrincipalId, request.TenantId);
            await _servicePrincipalService.EnableServicePrincipalAsync(request.TenantId, request.ServicePrincipalId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to enable service principal {ServicePrincipalId}", request.ServicePrincipalId);
            throw new InvalidOperationException($"Failed to enable service principal: {ex.Message}", ex);
        }
    }
}
