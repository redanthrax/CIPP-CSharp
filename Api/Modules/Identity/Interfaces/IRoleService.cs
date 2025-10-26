using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IRoleService {
    Task<PagedResponse<RoleDto>> GetRolesAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<RoleDto?> GetRoleAsync(Guid tenantId, string roleId, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(Guid tenantId, AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(Guid tenantId, string userId, string roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetRoleMembersAsync(Guid tenantId, string roleId, CancellationToken cancellationToken = default);
}