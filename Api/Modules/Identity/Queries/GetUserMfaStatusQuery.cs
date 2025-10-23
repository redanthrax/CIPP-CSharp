using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetUserMfaStatusQuery(
    string TenantId,
    string UserId
) : IRequest<GetUserMfaStatusQuery, Task<UserMfaStatusDto>>;