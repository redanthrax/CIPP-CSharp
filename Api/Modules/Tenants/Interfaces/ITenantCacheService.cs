using CIPP.Api.Modules.Tenants.Models;

namespace CIPP.Api.Modules.Tenants.Interfaces;

public interface ITenantCacheService
{
    Task<Tenant?> GetTenantAsync(Guid id);
    Task<Tenant?> GetTenantByTenantIdAsync(string tenantId);
    Task<List<Tenant>> GetTenantsAsync(string? filter = null, int skip = 0, int take = 50);
    Task SetTenantAsync(Tenant tenant, TimeSpan? expiry = null);
    Task RemoveTenantAsync(Guid id);
    Task RemoveTenantByTenantIdAsync(string tenantId);
    Task InvalidateTenantsListCache();
    Task<int> GetTenantCountAsync(string? filter = null);
    Task SetTenantCountAsync(int count, string? filter = null, TimeSpan? expiry = null);
    Task SetTenantsListAsync(List<Tenant> tenants, string? filter = null, int skip = 0, int take = 50, TimeSpan? expiry = null);
}
