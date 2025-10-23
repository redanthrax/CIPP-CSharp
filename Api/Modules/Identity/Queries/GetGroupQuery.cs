using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetGroupQuery(
    string TenantId,
    string GroupId
) : IRequest<GetGroupQuery, Task<GroupDto?>>;