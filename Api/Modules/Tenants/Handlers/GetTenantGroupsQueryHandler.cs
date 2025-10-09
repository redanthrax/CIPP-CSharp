using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Queries;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class GetTenantGroupsQueryHandler : IRequestHandler<GetTenantGroupsQuery, Task<List<TenantGroup>>> {
    private readonly ApplicationDbContext _context;

    public GetTenantGroupsQueryHandler(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<List<TenantGroup>> Handle(GetTenantGroupsQuery request, CancellationToken cancellationToken) {
        var query = _context.Set<TenantGroup>()
            .Include(g => g.Memberships)
            .ThenInclude(m => m.Tenant)
            .AsQueryable();

        if (request.GroupId.HasValue) {
            query = query.Where(g => g.Id == request.GroupId.Value);
        }

        return await query.OrderBy(g => g.Name).ToListAsync(cancellationToken);
    }
}