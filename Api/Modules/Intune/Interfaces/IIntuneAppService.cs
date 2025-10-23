using CIPP.Shared.DTOs.Intune;

namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IIntuneAppService {
    Task<List<IntuneAppDto>> GetAppsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<IntuneAppDto?> GetAppAsync(string tenantId, string appId, CancellationToken cancellationToken = default);
    Task AssignAppAsync(string tenantId, string appId, string assignTo, CancellationToken cancellationToken = default);
    Task DeleteAppAsync(string tenantId, string appId, CancellationToken cancellationToken = default);
}
