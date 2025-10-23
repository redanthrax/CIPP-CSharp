using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetServicePrincipalsQueryHandler : IRequestHandler<GetServicePrincipalsQuery, Task<PagedResponse<ServicePrincipalDto>>> {
    private readonly IServicePrincipalService _servicePrincipalService;
    private readonly ILogger<GetServicePrincipalsQueryHandler> _logger;

    public GetServicePrincipalsQueryHandler(
        IServicePrincipalService servicePrincipalService,
        ILogger<GetServicePrincipalsQueryHandler> logger) {
        _servicePrincipalService = servicePrincipalService;
        _logger = logger;
    }

    public async Task<PagedResponse<ServicePrincipalDto>> Handle(GetServicePrincipalsQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving service principals for tenant {TenantId}", request.TenantId);
        var result = await _servicePrincipalService.GetServicePrincipalsAsync(request.TenantId, request.Paging, cancellationToken);
        return result;
    }
}
