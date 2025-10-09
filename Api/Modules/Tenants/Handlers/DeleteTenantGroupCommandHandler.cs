using CIPP.Api.Data;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class DeleteTenantGroupCommandHandler : IRequestHandler<DeleteTenantGroupCommand, Task<bool>>
{
    private readonly ApplicationDbContext _context;

    public DeleteTenantGroupCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTenantGroupCommand request, CancellationToken cancellationToken)
    {
        var tenantGroup = await _context.Set<TenantGroup>()
            .Include(tg => tg.Memberships)
            .FirstOrDefaultAsync(tg => tg.Id == request.GroupId, cancellationToken);

        if (tenantGroup == null)
        {
            return false;
        }

        _context.Set<TenantGroupMembership>().RemoveRange(tenantGroup.Memberships);
        _context.Set<TenantGroup>().Remove(tenantGroup);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}