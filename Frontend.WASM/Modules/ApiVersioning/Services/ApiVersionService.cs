using CIPP.Frontend.WASM.Modules.ApiVersioning.Interfaces;

namespace CIPP.Frontend.WASM.Modules.ApiVersioning.Services;

public class ApiVersionService : IApiVersionService {
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiVersionService> _logger;

    public ApiVersionService(IConfiguration configuration, ILogger<ApiVersionService> logger) {
        _configuration = configuration;
        _logger = logger;
    }

    public string DefaultVersion => _configuration.GetValue<string>("Api:DefaultVersion") ?? "v1";

    public string GetVersionedUrl(string endpoint, string? version = null) {
        var apiVersion = version ?? DefaultVersion;
        var cleanEndpoint = endpoint.TrimStart('/').TrimEnd('/');
        
        if (IsInternalEndpoint(cleanEndpoint)) {
            return cleanEndpoint;
        }
        
        if (cleanEndpoint.StartsWith($"v{apiVersion.TrimStart('v')}/")) {
            return cleanEndpoint;
        }
        
        return $"v{apiVersion.TrimStart('v')}/{cleanEndpoint}";
    }

    public string GetNonVersionedUrl(string endpoint) {
        return endpoint.TrimStart('/').TrimEnd('/');
    }

    private bool IsInternalEndpoint(string endpoint) {
        var internalPrefixes = new[] { "microsoft", "swagger", "versioning", "authentication", "authorization" };
        return internalPrefixes.Any(prefix => endpoint.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}