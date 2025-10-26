using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
namespace CIPP.Api.Modules.MsGraph.Interfaces;
public interface IMicrosoftGraphService
{
    Task<GraphServiceClient> GetGraphServiceClientAsync(Guid? tenantId = null);
    Task<string> GetAccessTokenAsync(Guid tenantId, string scope);
    
    Task<Application?> GetApplicationAsync(string applicationId, Guid? tenantId = null);
    Task<DirectoryObject?> GetDirectoryObjectAsync(string objectId, Guid? tenantId = null);
    Task<Organization?> GetOrganizationAsync(Guid? tenantId = null);
    Task<ServicePrincipalCollectionResponse?> GetServicePrincipalsAsync(Guid? tenantId = null, string? filter = null, int? top = null);
    
    Task<ContractCollectionResponse?> GetPartnerTenantsAsync(string? filter = null, int? top = null, int? skip = null);
    Task<Contract?> GetPartnerTenantAsync(string contractId);
    Task<DomainCollectionResponse?> GetTenantDomainsAsync(Guid tenantId, string? filter = null);
    Task<bool> ValidateDomainAvailabilityAsync(string domainName);
}
