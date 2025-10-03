using Azure.Identity;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
namespace CIPP.Api.Modules.Microsoft.Services;
public class MicrosoftGraphService : IMicrosoftGraphService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MicrosoftGraphService> _logger;
    private readonly Dictionary<string, GraphServiceClient> _clientCache = new();
    public MicrosoftGraphService(IConfiguration configuration, ILogger<MicrosoftGraphService> logger)
    {
        _configuration = configuration;
        _logger = logger;
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
    public async Task<User?> GetUserAsync(string userId, string? tenantId = null)
    {
        try
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            return await graphClient.Users[userId].GetAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user {UserId} from tenant {TenantId}", userId, tenantId);
            return null;
        }
    }
    public async Task<UserCollectionResponse?> GetUsersAsync(string? tenantId = null, string? filter = null, int? top = null, string? select = null)
    {
        try
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            return await graphClient.Users.GetAsync(requestConfiguration =>
            {
                if (!string.IsNullOrEmpty(filter))
                    requestConfiguration.QueryParameters.Filter = filter;
                if (top.HasValue)
                    requestConfiguration.QueryParameters.Top = top.Value;
                if (!string.IsNullOrEmpty(select))
                    requestConfiguration.QueryParameters.Select = select.Split(',');
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users from tenant {TenantId}", tenantId);
            return null;
        }
    }
    public async Task<Application?> GetApplicationAsync(string applicationId, string? tenantId = null)
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
    }
    public async Task<DirectoryObject?> GetDirectoryObjectAsync(string objectId, string? tenantId = null)
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
    }
    public async Task<Organization?> GetOrganizationAsync(string? tenantId = null)
    {
        try
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            var organizations = await graphClient.Organization.GetAsync();
            return organizations?.Value?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get organization from tenant {TenantId}", tenantId);
            return null;
        }
    }
    public async Task<DeviceCollectionResponse?> GetDevicesAsync(
        string? tenantId = null, string? filter = null, int? top = null)
    {
        try
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            return await graphClient.Devices.GetAsync(requestConfiguration =>
            {
                if (!string.IsNullOrEmpty(filter))
                    requestConfiguration.QueryParameters.Filter = filter;
                if (top.HasValue)
                    requestConfiguration.QueryParameters.Top = top.Value;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get devices from tenant {TenantId}", tenantId);
            return null;
        }
    }
    public async Task<GroupCollectionResponse?> GetGroupsAsync(
        string? tenantId = null, string? filter = null, int? top = null)
    {
        try
        {
            var graphClient = await GetGraphServiceClientAsync(tenantId);
            return await graphClient.Groups.GetAsync(requestConfiguration =>
            {
                if (!string.IsNullOrEmpty(filter))
                    requestConfiguration.QueryParameters.Filter = filter;
                if (top.HasValue)
                    requestConfiguration.QueryParameters.Top = top.Value;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get groups from tenant {TenantId}", tenantId);
            return null;
        }
    }
    public async Task<ServicePrincipalCollectionResponse?> GetServicePrincipalsAsync(
        string? tenantId = null, string? filter = null, int? top = null)
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
    }

    // Partner/Customer tenant methods
    public async Task<ContractCollectionResponse?> GetPartnerTenantsAsync(
        string? filter = null, int? top = null, int? skip = null)
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
    }

    public async Task<Contract?> GetPartnerTenantAsync(string contractId)
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
    }

    public async Task<DomainCollectionResponse?> GetTenantDomainsAsync(string tenantId, string? filter = null)
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
    }
}
