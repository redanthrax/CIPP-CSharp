using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Queries;

public record GetTenantDashboardDataQuery(Guid TenantId)
    : IRequest<GetTenantDashboardDataQuery, Task<TenantDashboardData>>;
