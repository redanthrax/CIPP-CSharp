using Azure.Identity;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace CIPP.Api.Modules.MsGraph.Services;

public interface IGraphExceptionHandler
{
    Task<T?> HandleAsync<T>(Func<Task<T?>> operation, Guid? tenantId = null, string? operationDescription = null);
}

public class GraphExceptionHandler : IGraphExceptionHandler
{
    private readonly ILogger<GraphExceptionHandler> _logger;

    public GraphExceptionHandler(ILogger<GraphExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task<T?> HandleAsync<T>(Func<Task<T?>> operation, Guid? tenantId = null, string? operationDescription = null)
    {
        try
        {
            return await operation();
        }
        catch (MsalServiceException ex)
        {
            _logger.LogError(ex, "MSAL service exception{OperationDescription} for tenant {TenantId}: {ErrorCode}",
                string.IsNullOrEmpty(operationDescription) ? "" : $" when {operationDescription}", 
                tenantId, ex.ErrorCode);

            var errorDetails = new List<string>
            {
                $"MSAL Error Code: {ex.ErrorCode}",
                $"HTTP Status Code: {ex.StatusCode}"
            };

            if (!string.IsNullOrEmpty(ex.ResponseBody))
            {
                var parsedErrors = ParseAzureAdErrorResponse(ex.ResponseBody);
                if (parsedErrors.Any())
                {
                    errorDetails.AddRange(parsedErrors);
                }
                else
                {
                    errorDetails.Add($"Response Body: {ex.ResponseBody}");
                }
            }

            if (tenantId.HasValue)
            {
                errorDetails.Add($"Target Tenant: {tenantId}");
            }

            var detailedMessage = string.IsNullOrEmpty(operationDescription) 
                ? $"Microsoft Graph authentication failed: {ex.Message}"
                : $"Microsoft Graph authentication failed while {operationDescription}: {ex.Message}";

            var enhancedException = new InvalidOperationException(detailedMessage, ex);
            enhancedException.Data["ErrorDetails"] = errorDetails;

            throw enhancedException;
        }
        catch (AuthenticationFailedException ex)
        {
            _logger.LogError(ex, "Authentication failed{OperationDescription} for tenant {TenantId}",
                string.IsNullOrEmpty(operationDescription) ? "" : $" when {operationDescription}",
                tenantId);

            var errorDetails = new List<string>
            {
                $"Authentication Exception: {ex.GetType().Name}",
                $"Target Tenant: {tenantId?.ToString() ?? "default"}"
            };

            var msalException = FindMsalServiceException(ex);
            if (msalException != null && !string.IsNullOrEmpty(msalException.ResponseBody))
            {
                var parsedErrors = ParseAzureAdErrorResponse(msalException.ResponseBody);
                if (parsedErrors.Any())
                {
                    errorDetails.AddRange(parsedErrors);
                }
                else
                {
                    errorDetails.Add($"Response Body: {msalException.ResponseBody}");
                }
                
                errorDetails.Add($"MSAL Error Code: {msalException.ErrorCode}");
                errorDetails.Add($"HTTP Status Code: {msalException.StatusCode}");
            }

            var detailedMessage = string.IsNullOrEmpty(operationDescription)
                ? $"Azure Identity authentication failed: {ex.Message}"
                : $"Azure Identity authentication failed while {operationDescription}: {ex.Message}";

            var enhancedException = new InvalidOperationException(detailedMessage, ex);
            enhancedException.Data["ErrorDetails"] = errorDetails;

            throw enhancedException;
        }
        catch (Exception ex) when (ex.Data.Contains("ErrorDetails"))
        {
            throw;
        }
    }

    private static List<string> ParseAzureAdErrorResponse(string responseBody)
    {
        var errorDetails = new List<string>();

        try
        {
            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (root.TryGetProperty("error", out var errorElement) && errorElement.ValueKind == JsonValueKind.String)
            {
                errorDetails.Add($"Error Code: {errorElement.GetString()}");
            }

            if (root.TryGetProperty("error_description", out var descriptionElement) && descriptionElement.ValueKind == JsonValueKind.String)
            {
                errorDetails.Add($"Description: {descriptionElement.GetString()}");
            }

            if (root.TryGetProperty("error_codes", out var codesElement) && codesElement.ValueKind == JsonValueKind.Array)
            {
                var codes = codesElement.EnumerateArray()
                    .Where(e => e.ValueKind == JsonValueKind.Number)
                    .Select(e => e.GetInt32().ToString())
                    .ToList();

                if (codes.Any())
                {
                    errorDetails.Add($"Error Codes: [{string.Join(", ", codes)}]");
                }
            }

            if (root.TryGetProperty("trace_id", out var traceElement) && traceElement.ValueKind == JsonValueKind.String)
            {
                errorDetails.Add($"Trace ID: {traceElement.GetString()}");
            }

            if (root.TryGetProperty("correlation_id", out var correlationElement) && correlationElement.ValueKind == JsonValueKind.String)
            {
                errorDetails.Add($"Correlation ID: {correlationElement.GetString()}");
            }

            if (root.TryGetProperty("timestamp", out var timestampElement) && timestampElement.ValueKind == JsonValueKind.String)
            {
                errorDetails.Add($"Timestamp: {timestampElement.GetString()}");
            }
        }
        catch (JsonException) { }

        return errorDetails;
    }

    private static MsalServiceException? FindMsalServiceException(Exception exception)
    {
        var current = exception;
        while (current != null)
        {
            if (current is MsalServiceException msalException)
            {
                return msalException;
            }
            current = current.InnerException;
        }
        return null;
    }
}
