using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IRoleService {
    Task<PagedResponse<RoleDto>> GetRolesAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleAsync(string tenantId, string roleId, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(string tenantId, AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(string tenantId, string userId, string roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetRoleMembersAsync(string tenantId, string roleId, CancellationToken cancellationToken = default);
}