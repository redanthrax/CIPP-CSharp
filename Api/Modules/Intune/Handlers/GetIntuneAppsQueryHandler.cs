using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class GetIntuneAppsQueryHandler : IRequestHandler<GetIntuneAppsQuery, Task<List<IntuneAppDto>>> {
    private readonly IIntuneAppService _appService;

    public GetIntuneAppsQueryHandler(IIntuneAppService appService) {
        _appService = appService;
    }

    public async Task<List<IntuneAppDto>> Handle(GetIntuneAppsQuery query, CancellationToken cancellationToken) {
        return await _appService.GetAppsAsync(query.TenantId, cancellationToken);
    }
}
