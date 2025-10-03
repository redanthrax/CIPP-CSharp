using CIPP.Frontend.WASM.Modules.Authentication.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;

namespace CIPP.Frontend.WASM.Modules.Authentication.Services;

public class AuthenticationService : IAuthenticationService {
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;

    public AuthenticationService(
        AuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager) {
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
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
        _navigationManager.NavigateToLogin("authentication/login");
        await Task.CompletedTask;
    }

    public async Task SignOutAsync() {
        _navigationManager.NavigateToLogout("authentication/logout");
        await Task.CompletedTask;
    }

    public async Task RedirectToLoginAsync() {
        _navigationManager.NavigateToLogin("authentication/login");
        await Task.CompletedTask;
    }
}