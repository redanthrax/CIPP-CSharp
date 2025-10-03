using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
namespace CIPP.Api.Modules.Tenants.Queries;
public record GetTenantByIdQuery(Guid Id) : IRequest<GetTenantByIdQuery, Task<Tenant?>>;
