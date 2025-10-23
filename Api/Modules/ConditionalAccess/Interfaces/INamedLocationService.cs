using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;

namespace CIPP.Api.Modules.ConditionalAccess.Interfaces;

public interface INamedLocationService {
    Task<PagedResponse<NamedLocationDto>> GetNamedLocationsAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<NamedLocationDto?> GetNamedLocationAsync(string tenantId, string locationId, CancellationToken cancellationToken = default);
    Task<NamedLocationDto> CreateNamedLocationAsync(CreateNamedLocationDto createDto, CancellationToken cancellationToken = default);
    Task DeleteNamedLocationAsync(string tenantId, string locationId, CancellationToken cancellationToken = default);
}
