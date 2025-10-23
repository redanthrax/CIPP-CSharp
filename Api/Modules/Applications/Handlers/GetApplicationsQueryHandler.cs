using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, Task<PagedResponse<ApplicationDto>>> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetApplicationsQueryHandler> _logger;

    public GetApplicationsQueryHandler(
        IApplicationService applicationService,
        ILogger<GetApplicationsQueryHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<PagedResponse<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving applications for tenant {TenantId}", request.TenantId);
        var result = await _applicationService.GetApplicationsAsync(request.TenantId, request.Paging, cancellationToken);
        return result;
    }
}
