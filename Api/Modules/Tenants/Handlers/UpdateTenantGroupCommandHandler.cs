using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class UpdateTenantGroupCommandHandler : IRequestHandler<UpdateTenantGroupCommand, Task<TenantGroup>> {
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTenantGroupCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TenantGroup> Handle(UpdateTenantGroupCommand request, CancellationToken cancellationToken) {
        var existingGroup = await _context.GetEntitySet<TenantGroup>()
            .Include(g => g.Memberships)
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

        if (existingGroup == null) {
            throw new InvalidOperationException($"Tenant group with ID {request.GroupId} not found");
        }

        var currentUserId = _currentUserService.GetCurrentUserId() ?? Guid.Empty;
        existingGroup.Name = request.Name;
        existingGroup.Description = request.Description;
        existingGroup.UpdatedAt = DateTime.UtcNow;
        existingGroup.UpdatedBy = currentUserId;

        if (request.MemberTenantIds != null) {
            _context.GetEntitySet<TenantGroupMembership>().RemoveRange(existingGroup.Memberships);
            if (request.MemberTenantIds.Any()) {
                var newMemberships = request.MemberTenantIds.Select(tenantId => new TenantGroupMembership {
                    Id = Guid.NewGuid(),
                    TenantGroupId = existingGroup.Id,
                    TenantId = tenantId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserId
                }).ToList();

                await _context.GetEntitySet<TenantGroupMembership>().AddRangeAsync(newMemberships, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return await _context.GetEntitySet<TenantGroup>()
            .Include(g => g.Memberships)
            .FirstAsync(g => g.Id == request.GroupId, cancellationToken);
    }
}