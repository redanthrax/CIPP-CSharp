using CIPP.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text.Json;

namespace CIPP.Frontend.Modules.Authentication.Handlers;

public class ServerErrorHandler : DelegatingHandler {
    private readonly ILogger<ServerErrorHandler> _logger;
    private readonly NavigationManager _navigationManager;
    private readonly JsonSerializerOptions _jsonOptions;

    public ServerErrorHandler(ILogger<ServerErrorHandler> logger, NavigationManager navigationManager) {
        _logger = logger;
        _navigationManager = navigationManager;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var response = await base.SendAsync(request, cancellationToken);

        if ((int)response.StatusCode >= 400 && response.StatusCode != System.Net.HttpStatusCode.Unauthorized) {
            var endpoint = request.RequestUri?.PathAndQuery ?? "unknown";
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            
            _logger.LogError("Error response from API endpoint: {Endpoint}, Status: {StatusCode}", endpoint, response.StatusCode);
            
            try {
                var errorResponse = JsonSerializer.Deserialize<Response<object>>(content, _jsonOptions);

                if (errorResponse != null && !errorResponse.Success) {
                    var errorMessage = Uri.EscapeDataString(errorResponse.Message ?? $"Error {response.StatusCode}");
                    var errors = errorResponse.Errors != null && errorResponse.Errors.Any() 
                        ? Uri.EscapeDataString(string.Join("||", errorResponse.Errors))
                        : string.Empty;
                    
                    var errorUrl = $"/error?statusCode={(int)response.StatusCode}&endpoint={Uri.EscapeDataString(endpoint)}&message={errorMessage}";
                    if (!string.IsNullOrEmpty(errors)) {
                        errorUrl += $"&errors={errors}";
                    }
                    _navigationManager.NavigateTo(errorUrl);
                } else {
                    var errorMessage = Uri.EscapeDataString($"Server returned {response.StatusCode}");
                    _navigationManager.NavigateTo($"/error?statusCode={(int)response.StatusCode}&endpoint={Uri.EscapeDataString(endpoint)}&message={errorMessage}");
                }
            } catch (JsonException) {
                var errorMessage = Uri.EscapeDataString($"Server returned {response.StatusCode}");
                _navigationManager.NavigateTo($"/error?statusCode={(int)response.StatusCode}&endpoint={Uri.EscapeDataString(endpoint)}&message={errorMessage}");
            }
        }

        return response;
    }
}
