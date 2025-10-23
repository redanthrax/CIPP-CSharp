using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetNamedLocationQueryHandler : IRequestHandler<GetNamedLocationQuery, Task<NamedLocationDto?>> {
    private readonly INamedLocationService _locationService;

    public GetNamedLocationQueryHandler(INamedLocationService locationService) {
        _locationService = locationService;
    }

    public async Task<NamedLocationDto?> Handle(GetNamedLocationQuery query, CancellationToken cancellationToken) {
        return await _locationService.GetNamedLocationAsync(query.TenantId, query.LocationId, cancellationToken);
    }
}
