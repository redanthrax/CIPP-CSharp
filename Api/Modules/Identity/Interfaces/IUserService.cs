using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Api.Modules.Identity.Interfaces;

public interface IUserService {
    Task<PagedResponse<UserDto>> GetUsersAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateUserAsync(string tenantId, string userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task<string> ResetUserPasswordAsync(string tenantId, string userId, ResetUserPasswordDto resetPasswordDto, CancellationToken cancellationToken = default);
    Task EnableUserMfaAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task DisableUserMfaAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task<UserMfaStatusDto> GetUserMfaStatusAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
}