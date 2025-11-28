using CIPP.Api.Modules.Alerts.Interfaces;
using Fluid;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class WebhookAlertActionHandler : IAlertActionHandler {
    private readonly IAlertTemplateService _templateService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebhookAlertActionHandler> _logger;
    private readonly FluidParser _fluidParser;

    public string ActionType => "webhook";

    public WebhookAlertActionHandler(
        IAlertTemplateService templateService,
        IHttpClientFactory httpClientFactory,
        ILogger<WebhookAlertActionHandler> logger) {
        _templateService = templateService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _fluidParser = new FluidParser();
    }

    public async Task ExecuteAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string? alertComment = null,
        Dictionary<string, string>? additionalParams = null,
        CancellationToken cancellationToken = default) {
        try {
            var webhookUrl = additionalParams?.GetValueOrDefault("webhookUrl");
            if (string.IsNullOrEmpty(webhookUrl)) {
                _logger.LogWarning("No webhook URL specified for webhook alert");
                return;
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            await ConfigureAuthenticationAsync(httpClient, additionalParams, cancellationToken);
            ConfigureHeaders(httpClient, additionalParams);

            var payload = await BuildPayloadAsync(enrichedData, tenantId, alertComment, additionalParams);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(webhookUrl, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                "Webhook alert sent successfully to {WebhookUrl} for tenant {TenantId}",
                webhookUrl, tenantId);
        } catch (Exception ex) {
            _logger.LogError(ex, 
                "Failed to send webhook alert for tenant {TenantId}",
                tenantId);
            throw;
        }
    }

    private async Task<string> BuildPayloadAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string? alertComment,
        Dictionary<string, string>? additionalParams) {
        var customTemplate = additionalParams?.GetValueOrDefault("payloadTemplate");
        
        if (!string.IsNullOrEmpty(customTemplate)) {
            if (_fluidParser.TryParse(customTemplate, out var template, out var error)) {
                var templateOptions = new TemplateOptions();
                templateOptions.MemberAccessStrategy.Register<Dictionary<string, object>>();
                
                var context = new TemplateContext(new {
                    tenant_id = tenantId,
                    alert_comment = alertComment ?? "",
                    data = enrichedData
                }, templateOptions);
                
                var rendered = await template.RenderAsync(context);
                return rendered;
            } else {
                _logger.LogWarning("Failed to parse custom payload template: {Error}", error);
            }
        }

        var operation = enrichedData.TryGetValue("Operation", out var op) ? op.ToString() : "Generic";
        var defaultPayload = await _templateService.GenerateAlertJsonAsync(
            enrichedData,
            tenantId,
            operation ?? "Generic",
            alertComment);

        return JsonSerializer.Serialize(defaultPayload);
    }

    private async Task ConfigureAuthenticationAsync(
        HttpClient httpClient,
        Dictionary<string, string>? additionalParams,
        CancellationToken cancellationToken) {
        var authType = additionalParams?.GetValueOrDefault("authType");
        if (string.IsNullOrEmpty(authType)) {
            return;
        }

        switch (authType.ToLowerInvariant()) {
            case "bearer":
                var bearerToken = additionalParams?.GetValueOrDefault("authToken");
                if (!string.IsNullOrEmpty(bearerToken)) {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                }
                break;

            case "basic":
                var username = additionalParams?.GetValueOrDefault("authUsername");
                var password = additionalParams?.GetValueOrDefault("authPassword");
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)) {
                    var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                }
                break;

            case "apikey":
                var apiKey = additionalParams?.GetValueOrDefault("authApiKey");
                var apiKeyHeader = additionalParams?.GetValueOrDefault("authApiKeyHeader") ?? "X-API-Key";
                if (!string.IsNullOrEmpty(apiKey)) {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(apiKeyHeader, apiKey);
                }
                break;

            case "oauth2":
                var tokenUrl = additionalParams?.GetValueOrDefault("authTokenUrl");
                var clientId = additionalParams?.GetValueOrDefault("authClientId");
                var clientSecret = additionalParams?.GetValueOrDefault("authClientSecret");
                var scope = additionalParams?.GetValueOrDefault("authScope");
                
                if (!string.IsNullOrEmpty(tokenUrl) && !string.IsNullOrEmpty(clientId)) {
                    var token = await GetOAuth2TokenAsync(
                        tokenUrl, 
                        clientId, 
                        clientSecret ?? "", 
                        scope ?? "",
                        cancellationToken);
                    
                    if (!string.IsNullOrEmpty(token)) {
                        httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                }
                break;
        }
    }

    private async Task<string?> GetOAuth2TokenAsync(
        string tokenUrl,
        string clientId,
        string clientSecret,
        string scope,
        CancellationToken cancellationToken) {
        try {
            var tokenClient = _httpClientFactory.CreateClient();
            var tokenRequest = new Dictionary<string, string> {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            if (!string.IsNullOrEmpty(scope)) {
                tokenRequest["scope"] = scope;
            }

            var response = await tokenClient.PostAsync(
                tokenUrl,
                new FormUrlEncodedContent(tokenRequest),
                cancellationToken);

            response.EnsureSuccessStatusCode();
            var tokenData = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
            return tokenData.GetProperty("access_token").GetString();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to obtain OAuth2 token from {TokenUrl}", tokenUrl);
            return null;
        }
    }

    private void ConfigureHeaders(HttpClient httpClient, Dictionary<string, string>? additionalParams) {
        var customHeaders = additionalParams?.GetValueOrDefault("headers");
        if (string.IsNullOrEmpty(customHeaders)) {
            return;
        }

        try {
            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(customHeaders);
            if (headers != null) {
                foreach (var header in headers) {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to parse custom headers, continuing without them");
        }
    }
}
