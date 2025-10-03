using CIPP.Frontend.Modules.Authentication.Exceptions;
using CIPP.Frontend.Modules.Authentication.Handlers;

namespace CIPP.Frontend.Modules.Authentication.Services;

public class UnauthorizedResponseService : IDisposable {
    private readonly IAuthenticationService _authenticationService;
    private readonly UnauthorizedResponseHandler _handler;
    private readonly ILogger<UnauthorizedResponseService> _logger;

    public UnauthorizedResponseService(
        IAuthenticationService authenticationService,
        UnauthorizedResponseHandler handler,
        ILogger<UnauthorizedResponseService> logger) {
        _authenticationService = authenticationService;
        _handler = handler;
        _logger = logger;

        _handler.UnauthorizedAccess += OnUnauthorizedAccess;
    }

    private async void OnUnauthorizedAccess(object? sender, UnauthorizedApiException e) {
        _logger.LogWarning("Unauthorized access detected for endpoint: {Endpoint}. Redirecting to login.", e.Endpoint);
        
        try {
            await _authenticationService.RedirectToLoginAsync();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to redirect to login after unauthorized response");
        }
    }

    public void Dispose() {
        _handler.UnauthorizedAccess -= OnUnauthorizedAccess;
    }
}