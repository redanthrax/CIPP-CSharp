using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, Task<ApplicationDto?>> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetApplicationQueryHandler> _logger;

    public GetApplicationQueryHandler(
        IApplicationService applicationService,
        ILogger<GetApplicationQueryHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<ApplicationDto?> Handle(GetApplicationQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving application {ApplicationId} for tenant {TenantId}", request.ApplicationId, request.TenantId);
        var result = await _applicationService.GetApplicationAsync(request.TenantId, request.ApplicationId, cancellationToken);
        return result;
    }
}
