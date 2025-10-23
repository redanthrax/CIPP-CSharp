using Azure.Identity;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Models.ODataErrors;
namespace CIPP.Api.Modules.MsGraph.Services;
public class MicrosoftGraphService : IMicrosoftGraphService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MicrosoftGraphService> _logger;
    private readonly IGraphExceptionHandler _exceptionHandler;
    private readonly CachedGraphRequestHandler _cacheHandler;
    private readonly Dictionary<string, GraphServiceClient> _clientCache = new();
    public MicrosoftGraphService(
        IConfiguration configuration, 
        ILogger<MicrosoftGraphService> logger,
        IGraphExceptionHandler exceptionHandler,
        CachedGraphRequestHandler cacheHandler)
    {
        _configuration = configuration;
        _logger = logger;
        _exceptionHandler = exceptionHandler;
        _cacheHandler = cacheHandler;
    }
    public Task<GraphServiceClient> GetGraphServiceClientAsync(string? tenantId = null)
    {
        var targetTenantId = tenantId ?? _configuration["Authentication:AzureAd:TenantId"];
        if (string.IsNullOrEmpty(targetTenantId))
        {
            throw new InvalidOperationException("Tenant ID is required");
        }
        if (_clientCache.TryGetValue(targetTenantId, out var cachedClient))
        {
            return Task.FromResult(cachedClient);
        }
        var clientId = _configuration["Authentication:AzureAd:ClientId"];
        var clientSecret = _configuration["Authentication:AzureAd:ClientSecret"];
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new InvalidOperationException("Azure AD configuration is missing");
        }
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var clientSecretCredential = new ClientSecretCredential(targetTenantId, clientId, clientSecret);
        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        _clientCache[targetTenantId] = graphClient;
        return Task.FromResult(graphClient);
    }

    public async Task<string> GetAccessTokenAsync(string tenantId, string scope)
    {
        var clientId = _configuration["Authentication:AzureAd:ClientId"];
        var clientSecret = _configuration["Authentication:AzureAd:ClientSecret"];
        
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new InvalidOperationException("Azure AD configuration is missing");
        }

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        var tokenResult = await credential.GetTokenAsync(
            new Azure.Core.TokenRequestContext(new[] { scope }),
            CancellationToken.None
        );

        return tokenResult.Token;
    }
    public async Task<Application?> GetApplicationAsync(string applicationId, string? tenantId = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                var graphClient = await GetGraphServiceClientAsync(tenantId);
                return await graphClient.Applications[applicationId].GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get application {ApplicationId} from tenant {TenantId}", applicationId, tenantId);
                return null;
            }
        }, tenantId, $"getting application {applicationId}");
    }
    public async Task<DirectoryObject?> GetDirectoryObjectAsync(string objectId, string? tenantId = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                var graphClient = await GetGraphServiceClientAsync(tenantId);
                return await graphClient.DirectoryObjects[objectId].GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Failed to get directory object {ObjectId} from tenant {TenantId}", objectId, tenantId);
                return null;
            }
        }, tenantId, $"getting directory object {objectId}");
    }
    public async Task<Organization?> GetOrganizationAsync(string? tenantId = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            var organizations = await graphClient.Organization.GetAsync();
            return organizations?.Value?.FirstOrDefault();
        }, tenantId, "getting organization");
    }
    public async Task<ServicePrincipalCollectionResponse?> GetServicePrincipalsAsync(
        string? tenantId = null, string? filter = null, int? top = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                var graphClient = await GetGraphServiceClientAsync(tenantId);
                return await graphClient.ServicePrincipals.GetAsync(requestConfiguration =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (top.HasValue)
                        requestConfiguration.QueryParameters.Top = top.Value;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get service principals from tenant {TenantId}", tenantId);
                return null;
            }
        }, tenantId, "getting service principals");
    }

    public async Task<ContractCollectionResponse?> GetPartnerTenantsAsync(
        string? filter = null, int? top = null, int? skip = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving partner tenants with filter: {Filter}, top: {Top}, skip: {Skip}", 
                    filter, top, skip);

                var graphClient = await GetGraphServiceClientAsync();
                
                var response = await graphClient.Contracts.GetAsync(requestConfiguration =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (top.HasValue)
                        requestConfiguration.QueryParameters.Top = top.Value;
                    if (skip.HasValue)
                        requestConfiguration.QueryParameters.Skip = skip.Value;
                    
                    requestConfiguration.QueryParameters.Select = new[] {
                        "contractType",
                        "customerId", 
                        "defaultDomainName",
                        "displayName"
                    };
                });

                _logger.LogInformation("Successfully retrieved {Count} partner tenants", 
                    response?.Value?.Count ?? 0);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve partner tenants");
                return null;
            }
        }, null, "getting partner tenants");
    }

    public async Task<Contract?> GetPartnerTenantAsync(string contractId)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving partner tenant with contract ID: {ContractId}", contractId);

                var graphClient = await GetGraphServiceClientAsync();
                return await graphClient.Contracts[contractId].GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Failed to retrieve partner tenant with contract ID: {ContractId}", contractId);
                return null;
            }
        }, null, $"getting partner tenant {contractId}");
    }

    public async Task<DomainCollectionResponse?> GetTenantDomainsAsync(string tenantId, string? filter = null)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving domains for tenant: {TenantId} with filter: {Filter}", 
                    tenantId, filter);

                var graphClient = await GetGraphServiceClientAsync(tenantId);
                
                return await graphClient.Domains.GetAsync(requestConfiguration =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    
                    requestConfiguration.QueryParameters.Select = new[] {
                        "id",
                        "authenticationType", 
                        "isDefault",
                        "isInitial",
                        "isVerified",
                        "supportedServices"
                    };
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve domains for tenant: {TenantId}", tenantId);
                return null;
            }
        }, tenantId, $"getting domains for tenant {tenantId}");
    }

    public async Task<bool> ValidateDomainAvailabilityAsync(string domainName)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            try
            {
                _logger.LogInformation("Validating domain availability for: {DomainName}", domainName);
                var graphClient = await GetGraphServiceClientAsync();
                var response = await graphClient.Domains[domainName].GetAsync();
                _logger.LogInformation("Domain {DomainName} is already taken", domainName);
                return false;
            }
            catch (ODataError ex) when (ex.Error?.Code == "Request_ResourceNotFound")
            {
                _logger.LogInformation("Domain {DomainName} appears to be available", domainName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to validate domain availability for {DomainName}, assuming not available", domainName);
                return false;
            }
        }, null, $"validating domain availability for {domainName}");
    }

}
