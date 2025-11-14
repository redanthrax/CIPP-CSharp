using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Frontend.Modules.ApiVersioning.Interfaces;
using CIPP.Shared.DTOs;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CIPP.Frontend.Modules.Authentication.Services;

public class CippApiClient : ICippApiClient {
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _tokenProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CippApiClient> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApiVersionService _apiVersionService;
    private readonly JsonSerializerOptions _jsonOptions;

    public CippApiClient(
        HttpClient httpClient,
        IAccessTokenProvider tokenProvider,
        IConfiguration configuration,
        ILogger<CippApiClient> logger,
        IAuthenticationService authenticationService,
        IApiVersionService apiVersionService) {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
        _configuration = configuration;
        _logger = logger;
        _authenticationService = authenticationService;
        _apiVersionService = apiVersionService;

        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private async Task PrepareAuthenticatedRequest() {
        try {
            var scopes = _configuration.GetSection("Api:Scopes").Get<string[]>() ?? Array.Empty<string>();
            var tokenResult = await _tokenProvider.RequestAccessToken(new AccessTokenRequestOptions {
                Scopes = scopes
            });

            if (tokenResult.TryGetToken(out var token)) {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
            else {
                _logger.LogWarning("Failed to acquire access token");
                await _authenticationService.RedirectToLoginAsync();
                throw new UnauthorizedAccessException("Unable to acquire access token");
            }
        }
        catch (AccessTokenNotAvailableException ex) {
            _logger.LogWarning("Access token not available: {Message}", ex.Message);
            await _authenticationService.RedirectToLoginAsync();
            throw;
        }
    }

    public async Task<Response<T>> GetAsync<T>(string endpoint, string? apiVersion = null) {
        try {
            await PrepareAuthenticatedRequest();
            var versionedEndpoint = _apiVersionService.GetVersionedUrl(endpoint, apiVersion);
            var response = await _httpClient.GetAsync(versionedEndpoint);
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(content, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("API returned {StatusCode}: {Content}", response.StatusCode, content);
                return ParseErrorResponse<T>(content, response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling GET {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<PagedResponse<T>> GetPagedAsync<T>(string endpoint, int pageNumber = 1, int pageSize = 50, bool noCache = false, string? apiVersion = null) {
        try {
            await PrepareAuthenticatedRequest();
            var versionedEndpoint = _apiVersionService.GetVersionedUrl(endpoint, apiVersion);
            var separator = versionedEndpoint.Contains('?') ? '&' : '?';
            var url = $"{versionedEndpoint}{separator}pageNumber={pageNumber}&pageSize={pageSize}&noCache={noCache}";
            _logger.LogDebug("Making paged GET request to {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var wrappedResult = JsonSerializer.Deserialize<Response<PagedResponse<T>>>(content, _jsonOptions);
                if (wrappedResult?.Success == true && wrappedResult.Data != null) {
                    return wrappedResult.Data;
                }
                else {
                    _logger.LogWarning("Paged API returned unsuccessful response: {Message}", wrappedResult?.Message);
                    return new PagedResponse<T>();
                }
            }
            else {
                _logger.LogWarning("Paged API returned {StatusCode}: {Content}", response.StatusCode, content);
                return new PagedResponse<T>();
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling paged GET {Endpoint}", endpoint);
            return new PagedResponse<T>();
        }
    }

    public async Task<Response<T>> PostAsync<T>(string endpoint, object data, string? apiVersion = null) {
        try {
            await PrepareAuthenticatedRequest();
            var versionedEndpoint = _apiVersionService.GetVersionedUrl(endpoint, apiVersion);
            _logger.LogDebug("Making POST request to {Endpoint}", versionedEndpoint);
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(versionedEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(responseContent, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("POST API returned {StatusCode}: {Content}", response.StatusCode, responseContent);
                return ParseErrorResponse<T>(responseContent, response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling POST {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<Response<T>> PutAsync<T>(string endpoint, object data, string? apiVersion = null) {
        try {
            await PrepareAuthenticatedRequest();
            var versionedEndpoint = _apiVersionService.GetVersionedUrl(endpoint, apiVersion);
            _logger.LogDebug("Making PUT request to {Endpoint}", versionedEndpoint);
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync(versionedEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(responseContent, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("PUT API returned {StatusCode}: {Content}", response.StatusCode, responseContent);
                return ParseErrorResponse<T>(responseContent, response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling PUT {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<Response<bool>> DeleteAsync(string endpoint, string? apiVersion = null) {
        try {
            await PrepareAuthenticatedRequest();
            var versionedEndpoint = _apiVersionService.GetVersionedUrl(endpoint, apiVersion);
            _logger.LogDebug("Making DELETE request to {Endpoint}", versionedEndpoint);
            
            var response = await _httpClient.DeleteAsync(versionedEndpoint);
            
            if (response.IsSuccessStatusCode) {
                return Response<bool>.SuccessResult(true, "Resource deleted successfully");
            }
            else {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("DELETE API returned {StatusCode}: {Content}", response.StatusCode, content);
                return ParseErrorResponse<bool>(content, response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling DELETE {Endpoint}", endpoint);
            return Response<bool>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    private static Response<T> ParseErrorResponse<T>(string responseContent, System.Net.HttpStatusCode statusCode, string? reasonPhrase)
    {
        try
        {
            var errorResponse = JsonSerializer.Deserialize<Response<T>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (errorResponse != null)
            {
                return errorResponse;
            }
        }
        catch (JsonException)
        {
        }

        var message = $"API returned {statusCode}: {reasonPhrase ?? statusCode.ToString()}";
        var errors = new List<string>();

        if (!string.IsNullOrEmpty(responseContent))
        {
            if (responseContent.TrimStart().StartsWith("<"))
            {
                errors.Add($"Server returned HTML response (Status: {statusCode})");
            }
            else if (responseContent.Length > 200)
            {
                errors.Add($"Response: {responseContent[..200]}...");
            }
            else
            {
                errors.Add($"Response: {responseContent}");
            }
        }

        return Response<T>.ErrorResult(message, errors);
    }
}
