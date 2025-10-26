using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Queries;

public record GetTenantDetailsQuery(Guid TenantId)
    : IRequest<GetTenantDetailsQuery, Task<TenantDetailsDto>>;
