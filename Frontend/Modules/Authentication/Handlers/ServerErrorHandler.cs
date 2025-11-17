using CIPP.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Sockets;
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
        try {
            var response = await base.SendAsync(request, cancellationToken);

            if ((int)response.StatusCode >= 400 && response.StatusCode != System.Net.HttpStatusCode.Unauthorized) {
                var endpoint = request.RequestUri?.PathAndQuery ?? "unknown";
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                
                _logger.LogError("Error response from API endpoint: {Endpoint}, Status: {StatusCode}, Content: {Content}", 
                    endpoint, response.StatusCode, content);
                
                try {
                    var errorResponse = JsonSerializer.Deserialize<Response<object>>(content, _jsonOptions);
                    if (errorResponse != null && !errorResponse.Success && errorResponse.Errors?.Any() == true) {
                        _logger.LogError("API Errors: {Errors}", string.Join("; ", errorResponse.Errors));
                    }
                } catch (JsonException) {
                    _logger.LogWarning("Failed to deserialize error response content");
                }
            }

            return response;
        } catch (HttpRequestException ex) {
            _logger.LogError(ex, "Failed to connect to API: {Message}", ex.Message);
            
            var errorDetails = Uri.EscapeDataString($"Exception: {ex.GetType().Name}\nMessage: {ex.Message}\n\n{ex}");
            _navigationManager.NavigateTo($"/api-connection-error?errorDetails={errorDetails}");
            
            throw;
        } catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested) {
            _logger.LogError(ex, "API request timeout");
            
            var errorDetails = Uri.EscapeDataString($"Request Timeout\n\nThe API server did not respond within the expected time.");
            _navigationManager.NavigateTo($"/api-connection-error?errorDetails={errorDetails}");
            
            throw;
        } catch (SocketException ex) {
            _logger.LogError(ex, "Network socket error: {Message}", ex.Message);
            
            var errorDetails = Uri.EscapeDataString($"Socket Error: {ex.SocketErrorCode}\nMessage: {ex.Message}");
            _navigationManager.NavigateTo($"/api-connection-error?errorDetails={errorDetails}");
            
            throw;
        }
    }
}
