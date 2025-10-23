using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Frontend.Modules.Identity.Interfaces;

public interface IIdentityUserService {
    Task<Response<List<UserDto>>> GetUsersAsync(string tenantId);
    Task<Response<UserDto?>> GetUserAsync(string tenantId, string userId);
    Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto);
    Task<Response<UserDto>> UpdateUserAsync(string tenantId, string userId, UpdateUserDto userDto);
    Task<Response<object>> DeleteUserAsync(string tenantId, string userId);
    Task<Response<string>> ResetUserPasswordAsync(string tenantId, string userId, ResetUserPasswordDto passwordDto);
    Task<Response<object>> EnableUserMfaAsync(string tenantId, string userId);
    Task<Response<object>> DisableUserMfaAsync(string tenantId, string userId);
    Task<Response<UserMfaStatusDto>> GetUserMfaStatusAsync(string tenantId, string userId);
}