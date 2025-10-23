using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IServicePrincipalService {
    Task<PagedResponse<ServicePrincipalDto>> GetServicePrincipalsAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<ServicePrincipalDto?> GetServicePrincipalAsync(string tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task<ServicePrincipalDto> UpdateServicePrincipalAsync(string tenantId, string servicePrincipalId, UpdateServicePrincipalDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteServicePrincipalAsync(string tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task EnableServicePrincipalAsync(string tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task DisableServicePrincipalAsync(string tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
}
