using CIPP.Api.Modules.Frontend.TenantManagement.Models;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Interfaces;

public interface ITenantDashboardService {
    Task<TenantDashboardData> GetTenantDashboardDataAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<TenantHealthStatus> GetTenantHealthStatusAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<int> GetActiveUsersCountAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<int> GetUsedLicensesCountAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task RefreshTenantDataAsync(Guid tenantId, CancellationToken cancellationToken = default);
}