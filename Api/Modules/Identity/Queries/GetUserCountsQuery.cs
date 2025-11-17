using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetUserCountsQuery(Guid TenantId)
    : IRequest<GetUserCountsQuery, Task<UserCountsDto?>>;
