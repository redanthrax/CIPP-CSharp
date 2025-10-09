using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Task<Tenant>> {
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTenantCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Tenant> Handle(UpdateTenantCommand request, CancellationToken cancellationToken) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Tenant with ID {request.Id} not found");

        if (!string.IsNullOrEmpty(request.TenantAlias)) {
            if (string.IsNullOrEmpty(tenant.OriginalDisplayName)) {
                tenant.OriginalDisplayName = tenant.DisplayName;
            }
            tenant.TenantAlias = request.TenantAlias;
            tenant.DisplayName = request.TenantAlias;
        } else if (!string.IsNullOrEmpty(tenant.TenantAlias)) {
            tenant.TenantAlias = null;
            if (!string.IsNullOrEmpty(tenant.OriginalDisplayName)) {
                tenant.DisplayName = tenant.OriginalDisplayName;
                tenant.OriginalDisplayName = null;
            }
        }

        if (request.TenantGroups != null) {
            var existingMemberships = await _context.Set<TenantGroupMembership>()
                .Where(m => m.TenantId == tenant.Id)
                .ToListAsync(cancellationToken);

            _context.Set<TenantGroupMembership>().RemoveRange(existingMemberships);
            var currentUserId = _currentUserService.GetCurrentUserId() ?? Guid.Empty;
            var newMemberships = request.TenantGroups.Select(group => new TenantGroupMembership {
                Id = Guid.NewGuid(),
                TenantGroupId = group.GroupId,
                TenantId = tenant.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId
            }).ToList();

            await _context.Set<TenantGroupMembership>().AddRangeAsync(newMemberships, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return tenant;
    }
}