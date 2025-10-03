using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Authorization.Models;
using CIPP.Api.Modules.Authorization.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Services;

public class PermissionService : IPermissionService {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(
        ApplicationDbContext context,
        ILogger<PermissionService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<int> DiscoverAndRegisterPermissionsAsync() {
        try {
            _logger.LogInformation("Starting permission discovery from PermissionRegistry");
            var registeredPermissions = PermissionRegistry.GetAllPermissions();
            _logger.LogInformation("Discovered {Count} unique permissions", registeredPermissions.Count);
            var existingPermissions = await _context.GetEntitySet<Permission>()
                .ToListAsync();
            
            var permissionsToAdd = new List<Permission>();
            var permissionsToUpdate = new List<Permission>();
            
            foreach (var permissionInfo in registeredPermissions) {
                var existing = existingPermissions.FirstOrDefault(p => p.Name == permissionInfo.Permission);
                
                if (existing == null) {
                    permissionsToAdd.Add(new Permission {
                        Id = Guid.NewGuid(),
                        Name = permissionInfo.Permission,
                        Description = permissionInfo.Description,
                        Category = permissionInfo.Category,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    });
                }
                else if (existing.Description != permissionInfo.Description || 
                         existing.Category != permissionInfo.Category) {
                    existing.Description = permissionInfo.Description;
                    existing.Category = permissionInfo.Category;
                    permissionsToUpdate.Add(existing);
                }
            }
            
            if (permissionsToAdd.Any()) {
                await _context.GetEntitySet<Permission>().AddRangeAsync(permissionsToAdd);
            }
            
            if (permissionsToUpdate.Any()) {
                _context.GetEntitySet<Permission>().UpdateRange(permissionsToUpdate);
            }
            
            var changesCount = await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Permission discovery complete. Added: {Added}, Updated: {Updated}, Total Changes: {Changes}",
                permissionsToAdd.Count, permissionsToUpdate.Count, changesCount);
            return permissionsToAdd.Count + permissionsToUpdate.Count;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error during permission discovery");
            throw;
        }
    }

    public async Task<Role> EnsureSuperAdminRoleAsync() {
        try {
            var superAdminRole = await _context.GetEntitySet<Role>()
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == "Super Admin");
            
            if (superAdminRole == null) {
                superAdminRole = new Role {
                    Id = Guid.NewGuid(),
                    Name = "Super Admin",
                    Description = "Full system access with all permissions",
                    IsBuiltIn = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                
                await _context.GetEntitySet<Role>().AddAsync(superAdminRole);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Created Super Admin role");
            }
            
            var allPermissions = await _context.GetEntitySet<Permission>()
                .Where(p => p.IsActive)
                .ToListAsync();

            var currentPermissionIds = superAdminRole.RolePermissions
                .Select(rp => rp.PermissionId)
                .ToHashSet();
            
            var permissionsToAdd = allPermissions
                .Where(p => !currentPermissionIds.Contains(p.Id))
                .ToList();
            
            if (permissionsToAdd.Any()) {
                var rolePermissions = permissionsToAdd.Select(p => new RolePermission {
                    RoleId = superAdminRole.Id,
                    PermissionId = p.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                }).ToList();
                
                await _context.GetEntitySet<RolePermission>().AddRangeAsync(rolePermissions);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation(
                    "Added {Count} permissions to Super Admin role", permissionsToAdd.Count);
            }
            
            return superAdminRole;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error ensuring Super Admin role");
            throw;
        }
    }

    public async Task<ApplicationUser> HandleUserLoginAsync(string email, string displayName, 
        string? azureAdObjectId = null) {
        try {
            var existingUser = await _context.GetEntitySet<ApplicationUser>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (existingUser != null) {
                existingUser.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                return existingUser;
            }
            
            var userCount = await _context.GetEntitySet<ApplicationUser>().CountAsync();
            var isFirstUser = userCount == 0;
            var newUser = new ApplicationUser {
                Id = Guid.NewGuid(),
                Email = email,
                DisplayName = displayName,
                AzureAdObjectId = azureAdObjectId,
                IsFirstUser = isFirstUser,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };
            
            await _context.GetEntitySet<ApplicationUser>().AddAsync(newUser);
            
            if (isFirstUser) {
                var superAdminRole = await EnsureSuperAdminRoleAsync();
                var userRole = new UserRole {
                    UserId = newUser.Id,
                    RoleId = superAdminRole.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                
                await _context.GetEntitySet<UserRole>().AddAsync(userRole);
                
                _logger.LogInformation(
                    "First user {Email} logged in and assigned Super Admin role", email);
            }
            else {
                _logger.LogInformation(
                    "New user {Email} logged in with no roles assigned", email);
            }
            
            await _context.SaveChangesAsync();
            
            return await _context.GetEntitySet<ApplicationUser>()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstAsync(u => u.Id == newUser.Id);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error handling user login for {Email}", email);
            throw;
        }
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionName) {
        try {
            var hasPermission = await _context.GetEntitySet<UserRole>()
                .Where(ur => ur.UserId == userId)
                .Join(_context.GetEntitySet<RolePermission>(),
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(_context.GetEntitySet<Permission>(),
                    permissionId => permissionId,
                    p => p.Id,
                    (permissionId, p) => p.Name)
                .AnyAsync(name => name == permissionName);
            
            return hasPermission;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error checking permission {Permission} for user {UserId}", 
                permissionName, userId);
            return false;
        }
    }

    public async Task<List<string>> GetUserPermissionsAsync(Guid userId) {
        try {
            var permissions = await _context.GetEntitySet<UserRole>()
                .Where(ur => ur.UserId == userId)
                .Join(_context.GetEntitySet<RolePermission>(),
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(_context.GetEntitySet<Permission>(),
                    permissionId => permissionId,
                    p => p.Id,
                    (permissionId, p) => p.Name)
                .Distinct()
                .ToListAsync();
            
            return permissions;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error getting permissions for user {UserId}", userId);
            return new List<string>();
        }
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email) {
        return await _context.GetEntitySet<ApplicationUser>()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<Permission>> GetAllPermissionsAsync() {
        return await _context.GetEntitySet<Permission>()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Role>> GetAllRolesAsync() {
        return await _context.GetEntitySet<Role>()
            .Where(r => r.IsActive)
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId, string assignedBy) {
        try {
            var existingAssignment = await _context.GetEntitySet<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (existingAssignment != null) {
                return true;
            }
            
            var userRole = new UserRole {
                UserId = userId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = assignedBy
            };
            
            await _context.GetEntitySet<UserRole>().AddAsync(userRole);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}", roleId, userId);
            return false;
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId) {
        try {
            var userRole = await _context.GetEntitySet<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            
            if (userRole == null) {
                return true;
            }
            
            _context.GetEntitySet<UserRole>().Remove(userRole);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error removing role {RoleId} from user {UserId}", roleId, userId);
            return false;
        }
    }
}
