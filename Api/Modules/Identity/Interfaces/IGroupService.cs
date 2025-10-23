using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IGroupService {
    Task<PagedResponse<GroupDto>> GetGroupsAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<GroupDto?> GetGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, CancellationToken cancellationToken = default);
    Task<GroupDto> UpdateGroupAsync(string tenantId, string groupId, UpdateGroupDto updateGroupDto, CancellationToken cancellationToken = default);
    Task DeleteGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
    Task AddGroupMemberAsync(string tenantId, string groupId, AddGroupMemberDto addMemberDto, CancellationToken cancellationToken = default);
    Task RemoveGroupMemberAsync(string tenantId, string groupId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetGroupMembersAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetGroupOwnersAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
}