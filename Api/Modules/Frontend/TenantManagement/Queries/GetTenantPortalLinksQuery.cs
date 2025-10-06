using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Queries;

public record GetTenantPortalLinksQuery(Guid TenantId) : IRequest<GetTenantPortalLinksQuery, Task<PortalLinks>>;
