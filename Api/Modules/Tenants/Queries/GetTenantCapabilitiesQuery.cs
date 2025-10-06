using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Queries;

public record GetTenantCapabilitiesQuery(Guid TenantId) : IRequest<GetTenantCapabilitiesQuery, Task<TenantCapabilities?>>;
