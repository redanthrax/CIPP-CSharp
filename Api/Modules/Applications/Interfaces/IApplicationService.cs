using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IApplicationService {
    Task<PagedResponse<ApplicationDto>> GetApplicationsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<ApplicationDto?> GetApplicationAsync(Guid tenantId, string applicationId, CancellationToken cancellationToken = default);
    Task<ApplicationDto> UpdateApplicationAsync(Guid tenantId, string applicationId, UpdateApplicationDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteApplicationAsync(Guid tenantId, string applicationId, CancellationToken cancellationToken = default);
    Task<ApplicationCredentialDto> CreateApplicationCredentialAsync(CreateApplicationCredentialDto createCredentialDto, CancellationToken cancellationToken = default);
    Task DeleteApplicationCredentialAsync(DeleteApplicationCredentialDto deleteCredentialDto, CancellationToken cancellationToken = default);
}
