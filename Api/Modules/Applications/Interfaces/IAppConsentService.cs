using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IAppConsentService {
    Task<PagedResponse<AppConsentRequestDto>> GetAppConsentRequestsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
}
