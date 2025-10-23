using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetNamedLocationsQueryHandler : IRequestHandler<GetNamedLocationsQuery, Task<PagedResponse<NamedLocationDto>>> {
    private readonly INamedLocationService _locationService;

    public GetNamedLocationsQueryHandler(INamedLocationService locationService) {
        _locationService = locationService;
    }

    public async Task<PagedResponse<NamedLocationDto>> Handle(GetNamedLocationsQuery query, CancellationToken cancellationToken) {
        return await _locationService.GetNamedLocationsAsync(query.TenantId, query.Paging, cancellationToken);
    }
}
