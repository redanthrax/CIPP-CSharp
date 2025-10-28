using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Frontend.Modules.Identity.Interfaces;

public interface IIdentityUserService {
    Task<Response<List<UserDto>>> GetUsersAsync(Guid tenantId);
    Task<Response<UserDto?>> GetUserAsync(Guid tenantId, string userId);
    Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto);
    Task<Response<UserDto>> UpdateUserAsync(Guid tenantId, string userId, UpdateUserDto userDto);
    Task<Response<object>> DeleteUserAsync(Guid tenantId, string userId);
    Task<Response<string>> ResetUserPasswordAsync(Guid tenantId, string userId, ResetUserPasswordDto passwordDto);
    Task<Response<object>> EnableUserMfaAsync(Guid tenantId, string userId);
    Task<Response<object>> DisableUserMfaAsync(Guid tenantId, string userId);
    Task<Response<UserMfaStatusDto>> GetUserMfaStatusAsync(Guid tenantId, string userId);
}
