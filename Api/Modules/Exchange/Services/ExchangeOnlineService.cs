using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CIPP.Api.Modules.Exchange.Services;

public class ExchangeOnlineService : IExchangeOnlineService {
    private readonly ILogger<ExchangeOnlineService> _logger;
    private readonly IMicrosoftGraphService _graphService;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ExchangeResource = "https://outlook.office365.com";
    private const string ApiVersion = "beta";

    public ExchangeOnlineService(
        ILogger<ExchangeOnlineService> logger,
        IMicrosoftGraphService graphService,
        IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _graphService = graphService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T?> ExecuteCmdletAsync<T>(
        string tenantId,
        string cmdlet,
        Dictionary<string, object>? parameters = null,
        CancellationToken cancellationToken = default) where T : class {
        
        var results = await ExecuteCmdletListAsync<T>(tenantId, cmdlet, parameters, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<List<T>> ExecuteCmdletListAsync<T>(
        string tenantId,
        string cmdlet,
        Dictionary<string, object>? parameters = null,
        CancellationToken cancellationToken = default) where T : class {
        
        _logger.LogInformation("Executing Exchange Online cmdlet {Cmdlet} for tenant {TenantId}", cmdlet, tenantId);

        try {
            var response = await InvokeExchangeCmdletAsync(tenantId, cmdlet, parameters, cancellationToken);
            
            var results = new List<T>();
            if (response.Value != null) {
                foreach (var item in response.Value) {
                    try {
                        var json = JsonSerializer.Serialize(item);
                        var converted = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions {
                            PropertyNameCaseInsensitive = true
                        });
                        if (converted != null) {
                            results.Add(converted);
                        }
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to convert response item to type {Type}", typeof(T).Name);
                    }
                }
            }

            _logger.LogInformation("Cmdlet {Cmdlet} returned {Count} results", cmdlet, results.Count);
            return results;

        } catch (Exception ex) {
            _logger.LogError(ex, "Error executing Exchange Online cmdlet {Cmdlet} for tenant {TenantId}", cmdlet, tenantId);
            throw;
        }
    }

    public async Task ExecuteCmdletNoResultAsync(
        string tenantId,
        string cmdlet,
        Dictionary<string, object>? parameters = null,
        CancellationToken cancellationToken = default) {
        
        _logger.LogInformation("Executing Exchange Online cmdlet {Cmdlet} (no result) for tenant {TenantId}", cmdlet, tenantId);

        try {
            await InvokeExchangeCmdletAsync(tenantId, cmdlet, parameters, cancellationToken);
            _logger.LogInformation("Cmdlet {Cmdlet} executed successfully", cmdlet);

        } catch (Exception ex) {
            _logger.LogError(ex, "Error executing Exchange Online cmdlet {Cmdlet} for tenant {TenantId}", cmdlet, tenantId);
            throw;
        }
    }

    private async Task<ExchangeCommandResponse> InvokeExchangeCmdletAsync(
        string tenantId,
        string cmdlet,
        Dictionary<string, object>? parameters,
        CancellationToken cancellationToken) {
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var organization = await graphClient.Organization.GetAsync(cancellationToken: cancellationToken);
        var tenantCustomerId = organization?.Value?.FirstOrDefault()?.Id;

        if (string.IsNullOrEmpty(tenantCustomerId)) {
            throw new InvalidOperationException($"Could not retrieve tenant customer ID for {tenantId}");
        }

        var token = await _graphService.GetAccessTokenAsync(tenantId, $"{ExchangeResource}/.default");
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=1000");
        
        var systemMailboxGuid = "bb558c35-97f1-4cb9-8ff7-d53741dc928c";
        var anchor = $"APP:SystemMailbox{{{systemMailboxGuid}}}@{tenantCustomerId}";
        httpClient.DefaultRequestHeaders.Add("X-AnchorMailbox", anchor);

        var requestBody = new {
            CmdletInput = new {
                CmdletName = cmdlet,
                Parameters = parameters ?? new Dictionary<string, object>()
            }
        };

        var url = $"{ExchangeResource}/adminapi/{ApiVersion}/{tenantCustomerId}/InvokeCommand";
        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        _logger.LogDebug("POST {Url} - Cmdlet: {Cmdlet}", url, cmdlet);
        
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode) {
            _logger.LogError("Exchange Online API error: {StatusCode} - {Content}", response.StatusCode, responseContent);
            throw new HttpRequestException($"Exchange Online API error: {response.StatusCode} - {responseContent}");
        }

        var result = JsonSerializer.Deserialize<ExchangeCommandResponse>(responseContent, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        });

        return result ?? new ExchangeCommandResponse { Value = new List<JsonElement>() };
    }

    private class ExchangeCommandResponse {
        public List<JsonElement> Value { get; set; } = new();
    }
}
