using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IServicePrincipalService {
    Task<PagedResponse<ServicePrincipalDto>> GetServicePrincipalsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<ServicePrincipalDto?> GetServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task<ServicePrincipalDto> UpdateServicePrincipalAsync(Guid tenantId, string servicePrincipalId, UpdateServicePrincipalDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task EnableServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
    Task DisableServicePrincipalAsync(Guid tenantId, string servicePrincipalId, CancellationToken cancellationToken = default);
}
