using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetBpaRecommendationsQuery(Guid TenantId, string? Severity, string? Category)
    : IRequest<GetBpaRecommendationsQuery, Task<List<BpaRecommendationDto>>>;
