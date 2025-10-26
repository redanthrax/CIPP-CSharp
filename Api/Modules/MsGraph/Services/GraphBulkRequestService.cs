using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph;
using System.Text.Json;
using System.Text.Json.Nodes;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Models;
using GraphServiceClient = Microsoft.Graph.Beta.GraphServiceClient;

namespace CIPP.Api.Modules.MsGraph.Services;

public class GraphBulkRequestService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<GraphBulkRequestService> _logger;

    public GraphBulkRequestService(IMicrosoftGraphService graphService, ILogger<GraphBulkRequestService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<GraphBulkResponse>> ExecuteBulkRequestAsync(
        Guid tenantId, 
        List<GraphBulkRequestItem> requests,
        CancellationToken cancellationToken = default) {
        
        _logger.LogInformation("Executing bulk request with {RequestCount} items for tenant {TenantId}", 
            requests.Count, tenantId);

        if (!requests.Any()) {
            _logger.LogWarning("No requests provided for bulk execution");
            return new List<GraphBulkResponse>();
        }
        
        ValidateBatchRequests(requests);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var responses = new List<GraphBulkResponse>();

        const int batchSize = 20; // Microsoft Graph supports up to 20 requests per batch
        
        for (int i = 0; i < requests.Count; i += batchSize) {
            var batch = requests.Skip(i).Take(batchSize).ToList();
            var batchNumber = (i / batchSize) + 1;
            var totalBatches = (int)Math.Ceiling((double)requests.Count / batchSize);
            
            _logger.LogDebug("Processing batch {BatchNumber} of {TotalBatches}", batchNumber, totalBatches);
            
            var batchResponses = await ExecuteBatchAsync(graphClient, batch, cancellationToken);
            responses.AddRange(batchResponses);
        }
        
        _logger.LogInformation("Completed bulk request with {SuccessCount} successful and {FailureCount} failed responses",
            responses.Count(r => r.Status >= 200 && r.Status < 300),
            responses.Count(r => r.Status < 200 || r.Status >= 300));

        return responses;
    }

    private async Task<List<GraphBulkResponse>> ExecuteBatchAsync(
        GraphServiceClient graphClient, 
        List<GraphBulkRequestItem> requests,
        CancellationToken cancellationToken) {
        
        var responses = new List<GraphBulkResponse>();
        
        try {
            var batchRequestContentCollection = new BatchRequestContentCollection(graphClient);
            var requestIdMap = new Dictionary<string, string>();
            
            foreach (var request in requests) {
                var url = NormalizeUrl(request.Url);
                
                Microsoft.Kiota.Abstractions.Method httpMethod;
                switch (request.Method.ToUpper()) {
                    case "GET":
                        httpMethod = Microsoft.Kiota.Abstractions.Method.GET;
                        break;
                    case "POST":
                        httpMethod = Microsoft.Kiota.Abstractions.Method.POST;
                        break;
                    case "PUT":
                        httpMethod = Microsoft.Kiota.Abstractions.Method.PUT;
                        break;
                    case "PATCH":
                        httpMethod = Microsoft.Kiota.Abstractions.Method.PATCH;
                        break;
                    case "DELETE":
                        httpMethod = Microsoft.Kiota.Abstractions.Method.DELETE;
                        break;
                    default:
                        throw new ArgumentException($"Unsupported HTTP method: {request.Method}");
                }
                
                var requestInfo = new Microsoft.Kiota.Abstractions.RequestInformation {
                    HttpMethod = httpMethod,
                    UrlTemplate = $"{{+baseurl}}/{url}"
                };
                
                if (request.Headers.Count > 0) {
                    foreach (var header in request.Headers) {
                        requestInfo.Headers.Add(header.Key, header.Value);
                    }
                }
                
                if (!string.IsNullOrEmpty(request.Body) && request.Method.ToUpper() != "GET") {
                    requestInfo.SetContentFromScalar(graphClient.RequestAdapter, "application/json", request.Body);
                }
                
                var batchItemId = await batchRequestContentCollection.AddBatchRequestStepAsync(requestInfo);
                requestIdMap[request.Id] = batchItemId;
            }
            
            _logger.LogDebug("Sending batch request with {RequestCount} items", requests.Count);
            
            var batchResponseContentCollection = await graphClient.Batch.PostAsync(batchRequestContentCollection, cancellationToken);
            
            if (batchResponseContentCollection != null) {
                foreach (var request in requests) {
                    try {
                        var batchItemId = requestIdMap[request.Id];
                        var httpResponse = await batchResponseContentCollection.GetResponseByIdAsync(batchItemId);
                        
                        if (httpResponse != null) {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            var statusCode = (int)httpResponse.StatusCode;
                            
                            JsonElement body;
                            if (!string.IsNullOrEmpty(responseContent)) {
                                try {
                                    using var jsonDoc = JsonDocument.Parse(responseContent);
                                    body = jsonDoc.RootElement.Clone();
                                } catch {
                                    body = JsonSerializer.SerializeToElement(new { value = responseContent });
                                }
                            } else {
                                body = JsonSerializer.SerializeToElement(new { });
                            }
                            
                            responses.Add(new GraphBulkResponse {
                                Id = request.Id,
                                Status = statusCode,
                                Body = body
                            });
                        } else {
                            responses.Add(new GraphBulkResponse {
                                Id = request.Id,
                                Status = 500,
                                Body = JsonSerializer.SerializeToElement(new { error = new { message = "No response received" } })
                            });
                        }
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to get response for request {RequestId}", request.Id);
                        responses.Add(new GraphBulkResponse {
                            Id = request.Id,
                            Status = 500,
                            Body = JsonSerializer.SerializeToElement(new { error = new { message = ex.Message } })
                        });
                    }
                }
            } else {
                _logger.LogError("Batch request returned null response");
                responses = CreateErrorResponses(requests, 500, "Batch request returned null response");
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error executing batch request");
            responses = CreateErrorResponses(requests, 500, ex.Message);
        }
        
        return responses;
    }
    
    private object CreateBatchRequestBody(List<GraphBulkRequestItem> requests) {
        var batchRequests = requests.Select(request => {
            if (string.IsNullOrEmpty(request.Id)) {
                throw new ArgumentException($"Request ID cannot be null or empty");
            }
            
            if (string.IsNullOrEmpty(request.Url)) {
                throw new ArgumentException($"Request URL cannot be null or empty for ID: {request.Id}");
            }
            
            var url = NormalizeUrl(request.Url);
            var batchRequest = new {
                id = request.Id,
                method = request.Method.ToUpper(),
                url = url,
                headers = request.Headers.Count > 0 ? request.Headers : null,
                body = !string.IsNullOrEmpty(request.Body) ? ParseRequestBody(request.Body, request.Id) : null
            };
            return batchRequest;
        }).ToList();
        
        return new {
            requests = batchRequests
        };
    }
    
    private JsonNode? ParseRequestBody(string body, string requestId) {
        try {
            return JsonNode.Parse(body);
        }
        catch (JsonException ex) {
            _logger.LogWarning(ex, "Failed to parse request body for ID {RequestId}, sending as string", requestId);
            return JsonValue.Create(body);
        }
    }
    
    private List<GraphBulkResponse> ParseBatchResponse(string responseContent) {
        var responses = new List<GraphBulkResponse>();
        
        try {
            var jsonDoc = JsonDocument.Parse(responseContent);
            
            if (jsonDoc.RootElement.TryGetProperty("responses", out var responsesArray)) {
                foreach (var responseElement in responsesArray.EnumerateArray()) {
                    var response = new GraphBulkResponse();
                    
                    if (responseElement.TryGetProperty("id", out var idElement)) {
                        response.Id = idElement.GetString() ?? string.Empty;
                    }
                    
                    if (responseElement.TryGetProperty("status", out var statusElement)) {
                        response.Status = statusElement.GetInt32();
                    }
                    
                    if (responseElement.TryGetProperty("body", out var bodyElement)) {
                        response.Body = bodyElement.Clone();
                    } else {
                        response.Body = JsonSerializer.SerializeToElement(new { });
                    }
                    
                    responses.Add(response);
                }
            }
        }
        catch (JsonException ex) {
            _logger.LogError(ex, "Failed to parse batch response JSON");
            throw new InvalidOperationException("Failed to parse batch response", ex);
        }
        
        return responses;
    }
    
    private List<GraphBulkResponse> CreateErrorResponses(List<GraphBulkRequestItem> requests, int statusCode, string errorMessage) {
        return requests.Select(request => new GraphBulkResponse {
            Id = request.Id,
            Status = statusCode,
            Body = JsonSerializer.SerializeToElement(new {
                error = new {
                    code = statusCode.ToString(),
                    message = errorMessage
                }
            })
        }).ToList();
    }
    
    
    private string NormalizeUrl(string url) {
        if (url.StartsWith("https://graph.microsoft.com/beta/", StringComparison.OrdinalIgnoreCase)) {
            return url.Substring("https://graph.microsoft.com/beta/".Length);
        }
        
        if (url.StartsWith("https://graph.microsoft.com/v1.0/", StringComparison.OrdinalIgnoreCase)) {
            return url.Substring("https://graph.microsoft.com/v1.0/".Length);
        }
        
        if (url.StartsWith("/")) {
            return url.Substring(1);
        }
        
        return url;
    }
    
    private void ValidateBatchRequests(List<GraphBulkRequestItem> requests) {
        var duplicateIds = requests.GroupBy(r => r.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            
        if (duplicateIds.Any()) {
            throw new ArgumentException($"Duplicate request IDs found: {string.Join(", ", duplicateIds)}");
        }
        
        var invalidRequests = requests.Where(r => 
            string.IsNullOrWhiteSpace(r.Id) || 
            string.IsNullOrWhiteSpace(r.Url) ||
            string.IsNullOrWhiteSpace(r.Method)).ToList();
            
        if (invalidRequests.Any()) {
            var invalidIds = invalidRequests.Select(r => r.Id ?? "<null>").ToList();
            throw new ArgumentException($"Invalid requests found with IDs: {string.Join(", ", invalidIds)}");
        }
        
        if (requests.Count > 20) {
            throw new ArgumentException($"Batch size cannot exceed 20 requests. Current size: {requests.Count}");
        }
    }
}
