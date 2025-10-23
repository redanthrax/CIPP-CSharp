using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetUserQuery(
    string TenantId,
    string UserId
) : IRequest<GetUserQuery, Task<UserDto?>>;