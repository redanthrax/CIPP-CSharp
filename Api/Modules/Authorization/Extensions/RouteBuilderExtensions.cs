using CIPP.Api.Modules.Authorization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CIPP.Api.Modules.Authorization.Extensions;

public static class RouteBuilderExtensions {
    public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, 
        string permission, string description, string? category = null) {
        
        var permissionInfo = new PermissionInfo {
            Permission = permission,
            Description = description,
            Category = category ?? (permission.Contains('.') ? permission.Split('.')[0] : "General")
        };
        
        PermissionRegistry.RegisterPermission(permissionInfo);
        
        return builder.RequireAuthorization(policy => {
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new PermissionRequirement(permission));
        });
    }
    
    public static RouteGroupBuilder RequirePermission(this RouteGroupBuilder group, 
        string permission, string description, string? category = null) {
        
        var permissionInfo = new PermissionInfo {
            Permission = permission,
            Description = description,
            Category = category ?? (permission.Contains('.') ? permission.Split('.')[0] : "General")
        };
        
        PermissionRegistry.RegisterPermission(permissionInfo);
        
        return group.RequireAuthorization(policy => {
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new PermissionRequirement(permission));
        });
    }
}

public class PermissionInfo {
    public required string Permission { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
}

public static class PermissionRegistry {
    private static readonly List<PermissionInfo> _permissions = new();
    private static readonly object _lock = new();
    
    public static void RegisterPermission(PermissionInfo permissionInfo) {
        lock (_lock) {
            if (!_permissions.Any(p => p.Permission == permissionInfo.Permission)) {
                _permissions.Add(permissionInfo);
            }
        }
    }
    
    public static List<PermissionInfo> GetAllPermissions() {
        lock (_lock) {
            return _permissions.ToList();
        }
    }
    
    public static void Clear() {
        lock (_lock) {
            _permissions.Clear();
        }
    }
}

public class PermissionRequirement : IAuthorizationRequirement {
    public string Permission { get; }
    
    public PermissionRequirement(string permission) {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement> {
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApiKeyService _apiKeyService;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;
    
    public PermissionAuthorizationHandler(
        IPermissionService permissionService,
        ICurrentUserService currentUserService,
        IApiKeyService apiKeyService,
        ILogger<PermissionAuthorizationHandler> logger) {
        _permissionService = permissionService;
        _currentUserService = currentUserService;
        _apiKeyService = apiKeyService;
        _logger = logger;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement) {
        
        try {
            if (!_currentUserService.IsAuthenticated()) {
                _logger.LogWarning("User not authenticated for permission check: {Permission}", 
                    requirement.Permission);
                context.Fail();
                return;
            }

            var apiKeyClaim = context.User.FindFirst("cipp:api_key")?.Value;
            if (!string.IsNullOrEmpty(apiKeyClaim)) {
                var hasApiKeyPermission = await _apiKeyService.ApiKeyHasPermissionAsync(apiKeyClaim, requirement.Permission);
                
                if (hasApiKeyPermission) {
                    _logger.LogDebug("API key granted access - has permission: {Permission}", requirement.Permission);
                    context.Succeed(requirement);
                } else {
                    _logger.LogWarning("API key denied access - missing permission: {Permission}", requirement.Permission);
                    context.Fail();
                }
                return;
            }

            var currentUserId = _currentUserService.GetCurrentUserId();
            if (currentUserId == null) {
                var userEmail = _currentUserService.GetCurrentUserEmail();
                
                if (!string.IsNullOrEmpty(userEmail)) {
                    var displayName = _currentUserService.GetCurrentUserDisplayName() ?? userEmail;
                    var azureAdObjectId = _currentUserService.GetCurrentUserAzureAdObjectId();
                    var user = await _permissionService.HandleUserLoginAsync(userEmail, displayName, azureAdObjectId);
                    currentUserId = user.Id;
                    AddUserIdClaim(context.User, currentUserId.Value);
                    _logger.LogInformation("Created/updated user {Email} with ID {UserId}", 
                        userEmail, currentUserId);
                } else {
                    _logger.LogWarning("No user ID or email found for permission check");
                    context.Fail();
                    return;
                }
            }
            
            var hasPermission = await _permissionService.UserHasPermissionAsync(currentUserId.Value, requirement.Permission);
            if (hasPermission) {
                _logger.LogDebug("User {UserId} granted access - has permission: {Permission}", 
                    currentUserId, requirement.Permission);
                context.Succeed(requirement);
            } else {
                _logger.LogWarning("User {UserId} denied access - missing permission: {Permission}", 
                    currentUserId, requirement.Permission);
                context.Fail();
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error checking permission {Permission}", requirement.Permission);
            context.Fail();
        }
    }
    
    private static void AddUserIdClaim(ClaimsPrincipal user, Guid userId) {
        if (user.Identity is ClaimsIdentity identity) {
            if (!user.HasClaim("cipp:user_id", userId.ToString())) {
                identity.AddClaim(new Claim("cipp:user_id", userId.ToString()));
            }
        }
    }
}
