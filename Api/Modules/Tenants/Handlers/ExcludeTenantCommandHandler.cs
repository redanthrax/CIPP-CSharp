using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class ExcludeTenantCommandHandler : IRequestHandler<ExcludeTenantCommand, Task<string>> {
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ExcludeTenantCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(ExcludeTenantCommand request, CancellationToken cancellationToken) {
        var tenants = await _context.GetEntitySet<Tenant>()
            .Where(t => request.TenantIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        if (!tenants.Any()) {
            throw new InvalidOperationException("No tenants found with the specified IDs");
        }

        var currentUserId = _currentUserService.GetCurrentUserId() ?? Guid.Empty;
        var currentDate = DateTime.UtcNow;
        var updatedTenantNames = new List<string>();

        foreach (var tenant in tenants) {
            if (request.AddExclusion) {
                tenant.Excluded = true;
                tenant.ExcludeUser = currentUserId;
                tenant.ExcludeDate = currentDate;
            } else {
                tenant.Excluded = false;
                tenant.ExcludeUser = null;
                tenant.ExcludeDate = null;
            }
            updatedTenantNames.Add(tenant.DisplayName);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var action = request.AddExclusion ? "excluded" : "included";
        var tenantList = string.Join(", ", updatedTenantNames);
        
        return $"Successfully {action} tenant(s): {tenantList}";
    }
}