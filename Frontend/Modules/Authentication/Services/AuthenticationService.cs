using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CIPP.Frontend.Modules.Authentication;

public class AuthenticationService : IAuthenticationService {
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        AuthenticationStateProvider authenticationStateProvider,
        IHttpContextAccessor httpContextAccessor) {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> IsAuthenticatedAsync() {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<ClaimsPrincipal?> GetUserAsync() {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated == true ? authState.User : null;
    }

    public async Task<string?> GetUserNameAsync() {
        var user = await GetUserAsync();
        return user?.Identity?.Name;
    }

    public async Task<string?> GetUserEmailAsync() {
        var user = await GetUserAsync();
        return user?.FindFirst(ClaimTypes.Email)?.Value ??
               user?.FindFirst("preferred_username")?.Value;
    }

    public async Task<string[]> GetUserRolesAsync() {
        var user = await GetUserAsync();
        if (user == null) return Array.Empty<string>();
        
        return user.FindAll(ClaimTypes.Role)
                  .Select(c => c.Value)
                  .ToArray();
    }

    public async Task SignInAsync() {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null) {
            await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }

    public async Task SignOutAsync() {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null) {
            await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}