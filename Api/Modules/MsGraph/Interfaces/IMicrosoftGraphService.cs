using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
namespace CIPP.Api.Modules.MsGraph.Interfaces;
public interface IMicrosoftGraphService
{
    Task<GraphServiceClient> GetGraphServiceClientAsync(string? tenantId = null);
    Task<string> GetAccessTokenAsync(string tenantId, string scope);
    
    Task<Application?> GetApplicationAsync(string applicationId, string? tenantId = null);
    Task<DirectoryObject?> GetDirectoryObjectAsync(string objectId, string? tenantId = null);
    Task<Organization?> GetOrganizationAsync(string? tenantId = null);
    Task<ServicePrincipalCollectionResponse?> GetServicePrincipalsAsync(string? tenantId = null, string? filter = null, int? top = null);
    
    Task<ContractCollectionResponse?> GetPartnerTenantsAsync(string? filter = null, int? top = null, int? skip = null);
    Task<Contract?> GetPartnerTenantAsync(string contractId);
    Task<DomainCollectionResponse?> GetTenantDomainsAsync(string tenantId, string? filter = null);
    Task<bool> ValidateDomainAvailabilityAsync(string domainName);
}
