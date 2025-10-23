using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Queries;

public record GetNamedLocationQuery(
    string TenantId,
    string LocationId
) : IRequest<GetNamedLocationQuery, Task<NamedLocationDto?>>;
