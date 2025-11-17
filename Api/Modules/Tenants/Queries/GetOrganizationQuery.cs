using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Queries;

public record GetOrganizationQuery(Guid TenantId)
    : IRequest<GetOrganizationQuery, Task<OrganizationDto?>>;
