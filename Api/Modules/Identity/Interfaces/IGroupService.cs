using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IGroupService {
    Task<PagedResponse<GroupDto>> GetGroupsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<GroupDto?> GetGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, CancellationToken cancellationToken = default);
    Task<GroupDto> UpdateGroupAsync(Guid tenantId, string groupId, UpdateGroupDto updateGroupDto, CancellationToken cancellationToken = default);
    Task DeleteGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
    Task AddGroupMemberAsync(Guid tenantId, string groupId, AddGroupMemberDto addMemberDto, CancellationToken cancellationToken = default);
    Task RemoveGroupMemberAsync(Guid tenantId, string groupId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetGroupMembersAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetGroupOwnersAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
}