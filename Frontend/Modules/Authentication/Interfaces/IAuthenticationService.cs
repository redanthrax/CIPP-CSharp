using System.Security.Claims;

namespace CIPP.Frontend.Modules.Authentication.Interfaces;

public interface IAuthenticationService {
    Task<bool> IsAuthenticatedAsync();
    Task<ClaimsPrincipal?> GetUserAsync();
    Task<string?> GetUserNameAsync();
    Task<string?> GetUserEmailAsync();
    Task<string[]> GetUserRolesAsync();
    Task SignInAsync();
    Task SignOutAsync();
    Task RedirectToLoginAsync();
}
