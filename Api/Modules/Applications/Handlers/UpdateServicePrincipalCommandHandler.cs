using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class UpdateServicePrincipalCommandHandler : IRequestHandler<UpdateServicePrincipalCommand, Task<ServicePrincipalDto>> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<UpdateServicePrincipalCommandHandler> _logger;

    public UpdateServicePrincipalCommandHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<UpdateServicePrincipalCommandHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task<ServicePrincipalDto> Handle(UpdateServicePrincipalCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Updating service principal {ServicePrincipalId} for tenant {TenantId}", request.ServicePrincipalId, request.TenantId);
        return await _servicePrincipalService.UpdateServicePrincipalAsync(request.TenantId, request.ServicePrincipalId, request.ServicePrincipal, cancellationToken);
    }
}
