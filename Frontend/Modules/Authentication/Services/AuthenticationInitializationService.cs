namespace CIPP.Frontend.Modules.Authentication.Services;

public class AuthenticationInitializationService : IDisposable {
    private readonly UnauthorizedResponseService _unauthorizedResponseService;

    public AuthenticationInitializationService(UnauthorizedResponseService unauthorizedResponseService) {
        _unauthorizedResponseService = unauthorizedResponseService;
    }

    public void Dispose() {
        _unauthorizedResponseService?.Dispose();
    }
}