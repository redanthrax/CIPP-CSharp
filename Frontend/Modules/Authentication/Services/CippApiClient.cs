using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Shared.DTOs;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CIPP.Frontend.Modules.Authentication.Services;


public class CippApiClient : ICippApiClient {
    private readonly HttpClient _httpClient;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CippApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _baseUrl;
    private readonly string[] _scopes;

    public CippApiClient(
        HttpClient httpClient, 
        ITokenAcquisition tokenAcquisition, 
        IConfiguration configuration, 
        ILogger<CippApiClient> logger) {
        _httpClient = httpClient;
        _tokenAcquisition = tokenAcquisition;
        _configuration = configuration;
        _logger = logger;
        _baseUrl = _httpClient.BaseAddress?.ToString()!;
        _scopes = configuration.GetSection("DownstreamApi:Scopes").Get<string[]>() ?? Array.Empty<string>();
        
        _logger.LogInformation("CippApiClient configured with base URL: {BaseUrl} from HttpClient.BaseAddress: {HttpClientBaseAddress}", _baseUrl, _httpClient.BaseAddress);
        
        _jsonOptions = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private async Task PrepareAuthenticatedRequest() {
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<Response<T>> GetAsync<T>(string endpoint) {
        try {
            await PrepareAuthenticatedRequest();
            var response = await _httpClient.GetAsync(endpoint.TrimStart('/'));
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(content, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("API returned {StatusCode}: {Content}", response.StatusCode, content);
                return Response<T>.ErrorResult($"API returned {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling GET {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<PagedResponse<T>> GetPagedAsync<T>(string endpoint, int pageNumber = 1, int pageSize = 50, bool noCache = false) {
        try {
            await PrepareAuthenticatedRequest();
            var url = $"{endpoint.TrimStart('/')}?pageNumber={pageNumber}&pageSize={pageSize}&noCache={noCache}";
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

    public async Task<Response<T>> PostAsync<T>(string endpoint, object data) {
        try {
            await PrepareAuthenticatedRequest();
            _logger.LogDebug("Making POST request to {Endpoint}", endpoint);
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(endpoint.TrimStart('/'), content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(responseContent, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("POST API returned {StatusCode}: {Content}", response.StatusCode, responseContent);
                return Response<T>.ErrorResult($"API returned {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling POST {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<Response<T>> PutAsync<T>(string endpoint, object data) {
        try {
            await PrepareAuthenticatedRequest();
            _logger.LogDebug("Making PUT request to {Endpoint}", endpoint);
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync(endpoint.TrimStart('/'), content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode) {
                var result = JsonSerializer.Deserialize<Response<T>>(responseContent, _jsonOptions);
                return result ?? Response<T>.ErrorResult("Failed to deserialize response");
            }
            else {
                _logger.LogWarning("PUT API returned {StatusCode}: {Content}", response.StatusCode, responseContent);
                return Response<T>.ErrorResult($"API returned {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling PUT {Endpoint}", endpoint);
            return Response<T>.ErrorResult($"API call failed: {ex.Message}");
        }
    }

    public async Task<Response<bool>> DeleteAsync(string endpoint) {
        try {
            await PrepareAuthenticatedRequest();
            _logger.LogDebug("Making DELETE request to {Endpoint}", endpoint);
            
            var response = await _httpClient.DeleteAsync(endpoint.TrimStart('/'));
            
            if (response.IsSuccessStatusCode) {
                return Response<bool>.SuccessResult(true, "Resource deleted successfully");
            }
            else {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("DELETE API returned {StatusCode}: {Content}", response.StatusCode, content);
                return Response<bool>.ErrorResult($"API returned {response.StatusCode}: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error calling DELETE {Endpoint}", endpoint);
            return Response<bool>.ErrorResult($"API call failed: {ex.Message}");
        }
    }
}