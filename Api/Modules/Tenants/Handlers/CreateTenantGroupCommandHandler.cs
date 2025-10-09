using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class CreateTenantGroupCommandHandler : IRequestHandler<CreateTenantGroupCommand, Task<TenantGroup>> {
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTenantGroupCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TenantGroup> Handle(CreateTenantGroupCommand request, CancellationToken cancellationToken) {
        var existingGroup = await _context.Set<TenantGroup>()
            .FirstOrDefaultAsync(g => g.Name == request.Name, cancellationToken);

        if (existingGroup != null) {
            throw new InvalidOperationException($"A tenant group with name '{request.Name}' already exists");
        }

        var currentUserId = _currentUserService.GetCurrentUserId() ?? Guid.Empty;
        var tenantGroup = new TenantGroup {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserId
        };

        _context.Set<TenantGroup>().Add(tenantGroup);
        if (request.MemberTenantIds?.Any() == true) {
            var existingTenantIds = await _context.GetEntitySet<Tenant>()
                .Where(t => request.MemberTenantIds.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

            var invalidTenantIds = request.MemberTenantIds.Except(existingTenantIds).ToList();
            if (invalidTenantIds.Any()) {
                throw new InvalidOperationException($"Invalid tenant IDs: {string.Join(", ", invalidTenantIds)}");
            }

            var memberships = request.MemberTenantIds.Select(tenantId => new TenantGroupMembership {
                Id = Guid.NewGuid(),
                TenantGroupId = tenantGroup.Id,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId
            }).ToList();

            await _context.Set<TenantGroupMembership>().AddRangeAsync(memberships, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return tenantGroup;
    }
}