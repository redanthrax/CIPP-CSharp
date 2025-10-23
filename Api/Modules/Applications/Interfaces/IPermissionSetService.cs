using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IPermissionSetService {
    Task<PagedResponse<PermissionSetDto>> GetPermissionSetsAsync(PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<PermissionSetDto?> GetPermissionSetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PermissionSetDto> CreatePermissionSetAsync(CreatePermissionSetDto createDto, CancellationToken cancellationToken = default);
    Task<PermissionSetDto> UpdatePermissionSetAsync(Guid id, UpdatePermissionSetDto updateDto, CancellationToken cancellationToken = default);
    Task DeletePermissionSetAsync(Guid id, CancellationToken cancellationToken = default);
}
