using CIPP.Api.Modules.Authorization.Models;

namespace CIPP.Api.Modules.Authorization.Interfaces;

public interface IPermissionService
{

    Task<int> DiscoverAndRegisterPermissionsAsync();

    Task<Role> EnsureSuperAdminRoleAsync();

    Task<ApplicationUser> HandleUserLoginAsync(string email, string displayName, string? azureAdObjectId = null);

    Task<bool> UserHasPermissionAsync(Guid userId, string permissionName);

    Task<List<string>> GetUserPermissionsAsync(Guid userId);

    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    Task<List<Permission>> GetAllPermissionsAsync();

    Task<List<Role>> GetAllRolesAsync();

    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId, string assignedBy);

    Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
}
