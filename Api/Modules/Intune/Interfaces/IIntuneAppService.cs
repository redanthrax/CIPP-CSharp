using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IIntuneAppService {
    Task<List<IntuneAppDto>> GetAppsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IntuneAppDto?> GetAppAsync(Guid tenantId, string appId, CancellationToken cancellationToken = default);
    Task AssignAppAsync(Guid tenantId, string appId, string assignTo, CancellationToken cancellationToken = default);
    Task DeleteAppAsync(Guid tenantId, string appId, CancellationToken cancellationToken = default);
}
