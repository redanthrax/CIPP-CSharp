using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetBpaRecommendationsQueryHandler : IRequestHandler<GetBpaRecommendationsQuery, Task<List<BpaRecommendationDto>>> {
    private readonly IBpaService _bpaService;

    public GetBpaRecommendationsQueryHandler(IBpaService bpaService) {
        _bpaService = bpaService;
    }

    public async Task<List<BpaRecommendationDto>> Handle(GetBpaRecommendationsQuery request, CancellationToken cancellationToken = default) {
        return await _bpaService.GetRecommendationsAsync(request.TenantId, request.Severity, request.Category, cancellationToken);
    }
}
