using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IUserService {
    Task<PagedResponse<UserDto>> GetUsersAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateUserAsync(Guid tenantId, string userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task<string> ResetUserPasswordAsync(Guid tenantId, string userId, ResetUserPasswordDto resetPasswordDto, CancellationToken cancellationToken = default);
    Task EnableUserMfaAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task DisableUserMfaAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task<UserMfaStatusDto> GetUserMfaStatusAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
}