using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Frontend.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;

namespace CIPP.Frontend.Modules.Identity.Services;

public class IdentityUserService : IIdentityUserService {
    private readonly ICippApiClient _apiClient;
    private readonly ILogger<IdentityUserService> _logger;

    public IdentityUserService(ICippApiClient apiClient, ILogger<IdentityUserService> logger) {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<Response<List<UserDto>>> GetUsersAsync(string tenantId) {
        try {
            _logger.LogInformation("Getting users for tenant {TenantId}", tenantId);
            return await _apiClient.GetAsync<List<UserDto>>($"identity/users?tenantId={tenantId}");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error getting users for tenant {TenantId}", tenantId);
            return Response<List<UserDto>>.ErrorResult($"Failed to get users: {ex.Message}");
        }
    }

    public async Task<Response<UserDto?>> GetUserAsync(string tenantId, string userId) {
        try {
            _logger.LogInformation("Getting user {UserId} for tenant {TenantId}", userId, tenantId);
            return await _apiClient.GetAsync<UserDto?>($"identity/users/{userId}?tenantId={tenantId}");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error getting user {UserId} for tenant {TenantId}", userId, tenantId);
            return Response<UserDto?>.ErrorResult($"Failed to get user: {ex.Message}");
        }
    }

    public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto) {
        try {
            _logger.LogInformation("Creating user {UserPrincipalName} for tenant {TenantId}", 
                userDto.UserPrincipalName, userDto.TenantId);
            return await _apiClient.PostAsync<UserDto>("identity/users", userDto);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating user {UserPrincipalName}", userDto.UserPrincipalName);
            return Response<UserDto>.ErrorResult($"Failed to create user: {ex.Message}");
        }
    }

    public async Task<Response<UserDto>> UpdateUserAsync(string tenantId, string userId, UpdateUserDto userDto) {
        try {
            _logger.LogInformation("Updating user {UserId} for tenant {TenantId}", userId, tenantId);
            return await _apiClient.PutAsync<UserDto>($"identity/users/{userId}?tenantId={tenantId}", userDto);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error updating user {UserId} for tenant {TenantId}", userId, tenantId);
            return Response<UserDto>.ErrorResult($"Failed to update user: {ex.Message}");
        }
    }

    public async Task<Response<object>> DeleteUserAsync(string tenantId, string userId) {
        try {
            _logger.LogInformation("Deleting user {UserId} for tenant {TenantId}", userId, tenantId);
            var deleteResult = await _apiClient.DeleteAsync($"identity/users/{userId}?tenantId={tenantId}");
            return deleteResult.Success 
                ? Response<object>.SuccessResult(new object())
                : Response<object>.ErrorResult(deleteResult.Message ?? "Failed to delete user");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error deleting user {UserId} for tenant {TenantId}", userId, tenantId);
            return Response<object>.ErrorResult($"Failed to delete user: {ex.Message}");
        }
    }

    public async Task<Response<string>> ResetUserPasswordAsync(string tenantId, string userId, ResetUserPasswordDto passwordDto) {
        try {
            _logger.LogInformation("Resetting password for user {UserId} in tenant {TenantId}", userId, tenantId);
            return await _apiClient.PostAsync<string>($"identity/users/{userId}/reset-password", passwordDto);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error resetting password for user {UserId} in tenant {TenantId}", userId, tenantId);
            return Response<string>.ErrorResult($"Failed to reset password: {ex.Message}");
        }
    }

    public async Task<Response<object>> EnableUserMfaAsync(string tenantId, string userId) {
        try {
            _logger.LogInformation("Enabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
            return await _apiClient.PostAsync<object>($"identity/users/{userId}/enable-mfa?tenantId={tenantId}", new { });
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error enabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
            return Response<object>.ErrorResult($"Failed to enable MFA: {ex.Message}");
        }
    }

    public async Task<Response<object>> DisableUserMfaAsync(string tenantId, string userId) {
        try {
            _logger.LogInformation("Disabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
            return await _apiClient.PostAsync<object>($"identity/users/{userId}/disable-mfa?tenantId={tenantId}", new { });
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error disabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
            return Response<object>.ErrorResult($"Failed to disable MFA: {ex.Message}");
        }
    }

    public async Task<Response<UserMfaStatusDto>> GetUserMfaStatusAsync(string tenantId, string userId) {
        try {
            _logger.LogInformation("Getting MFA status for user {UserId} in tenant {TenantId}", userId, tenantId);
            return await _apiClient.GetAsync<UserMfaStatusDto>($"identity/users/{userId}/mfa-status?tenantId={tenantId}");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error getting MFA status for user {UserId} in tenant {TenantId}", userId, tenantId);
            return Response<UserMfaStatusDto>.ErrorResult($"Failed to get MFA status: {ex.Message}");
        }
    }
}