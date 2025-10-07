using CIPP.Frontend.Modules.Authentication.Exceptions;
using System.Net;

namespace CIPP.Frontend.Modules.Authentication.Handlers;

public class UnauthorizedResponseHandler : DelegatingHandler {
    private readonly ILogger<UnauthorizedResponseHandler> _logger;

    public event EventHandler<UnauthorizedApiException>? UnauthorizedAccess;

    public UnauthorizedResponseHandler(ILogger<UnauthorizedResponseHandler> logger) {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized) {
            var endpoint = request.RequestUri?.PathAndQuery ?? "unknown";
            _logger.LogWarning("Unauthorized response from API endpoint: {Endpoint}", endpoint);
            
            var exception = new UnauthorizedApiException(endpoint, (int)response.StatusCode);
            UnauthorizedAccess?.Invoke(this, exception);
        }

        return response;
    }
}
