using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetServicePrincipalQueryHandler : IRequestHandler<GetServicePrincipalQuery, Task<ServicePrincipalDto?>> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<GetServicePrincipalQueryHandler> _logger;

    public GetServicePrincipalQueryHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<GetServicePrincipalQueryHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task<ServicePrincipalDto?> Handle(GetServicePrincipalQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving service principal {ServicePrincipalId} for tenant {TenantId}", request.ServicePrincipalId, request.TenantId);
        var result = await _servicePrincipalService.GetServicePrincipalAsync(request.TenantId, request.ServicePrincipalId, cancellationToken);
        return result;
    }
}
