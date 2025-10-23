using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetAppConsentRequestsQueryHandler : IRequestHandler<GetAppConsentRequestsQuery, Task<PagedResponse<AppConsentRequestDto>>> {
    private readonly IAppConsentService _appConsentService;
    private readonly ILogger<GetAppConsentRequestsQueryHandler> _logger;

    public GetAppConsentRequestsQueryHandler(
        IAppConsentService appConsentService,
        ILogger<GetAppConsentRequestsQueryHandler> logger) {
        _appConsentService = appConsentService;
        _logger = logger;
    }

    public async Task<PagedResponse<AppConsentRequestDto>> Handle(GetAppConsentRequestsQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving app consent requests for tenant {TenantId}", request.TenantId);
        var result = await _appConsentService.GetAppConsentRequestsAsync(request.TenantId, request.Paging, cancellationToken);
        return result;
    }
}
