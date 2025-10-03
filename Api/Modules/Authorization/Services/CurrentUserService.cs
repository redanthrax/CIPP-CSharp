using System.Security.Claims;
using CIPP.Api.Modules.Authorization.Interfaces;

namespace CIPP.Api.Modules.Authorization.Services;

public class CurrentUserService : ICurrentUserService {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CurrentUserService> _logger;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CurrentUserService> logger) {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public Guid? GetCurrentUserId() {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) {
            return null;
        }

        var userIdClaim = user.FindFirst("cipp:user_id")?.Value;
        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId)) {
            return userId;
        }

        var subjectClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                          user.FindFirst("sub")?.Value;
        
        if (!string.IsNullOrEmpty(subjectClaim) && Guid.TryParse(subjectClaim, out var subjectGuid)) {
            return subjectGuid;
        }

        return null;
    }

    public string? GetCurrentUserEmail() {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) {
            return null;
        }

        return user.FindFirst(ClaimTypes.Email)?.Value ??
               user.FindFirst("email")?.Value ??
               user.FindFirst("preferred_username")?.Value ??
               user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetCurrentUserDisplayName() {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) {
            return null;
        }

        return user.FindFirst(ClaimTypes.GivenName)?.Value ??
               user.FindFirst("name")?.Value ??
               user.FindFirst("displayName")?.Value ??
               user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetCurrentUserAzureAdObjectId() {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) {
            return null;
        }

        return user.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ??
               user.FindFirst("oid")?.Value;
    }

    public bool IsAuthenticated() {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
    }
}
