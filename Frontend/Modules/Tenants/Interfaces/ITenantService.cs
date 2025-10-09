using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Frontend.Modules.Tenants.Interfaces;

public interface ITenantService {
    Task<PagedResponse<TenantDto>> GetTenantsAsync(int pageNumber = 1, int pageSize = 50, 
        bool allTenants = false, bool includeOffboardingDefaults = false, bool noCache = false);
    
    Task<Response<TenantDetailsDto>> GetTenantDetailsAsync(Guid tenantId);
    
    Task<Response<List<TenantSelectorOptionDto>>> GetTenantSelectorOptionsAsync(
        bool allTenants = false, bool includeGroups = false, bool includeOffboardingDefaults = false);
    
    Task<Response<ValidateDomainResponseDto>> ValidateTenantDomainAsync(string tenantName);
    
    Task<Response<TenantDto>> AddTenantAsync(AddTenantRequestDto request);
    
    Task<Response<TenantDetailsDto>> UpdateTenantAsync(EditTenantRequestDto request);
    
    Task<Response<bool>> DeleteTenantAsync(Guid tenantId);
    
    Task<Response<string>> ExcludeTenantAsync(List<Guid> tenantIds, bool addExclusion);
    
    Task<PagedResponse<TenantGroupDto>> GetTenantGroupsAsync(int pageNumber = 1, int pageSize = 50, 
        bool noCache = false);
    
    Task<Response<TenantGroupDto>> GetTenantGroupAsync(Guid groupId);
    
    Task<Response<TenantGroupDto>> AddTenantGroupAsync(CreateTenantGroupDto request);
    
    Task<Response<TenantGroupDto>> UpdateTenantGroupAsync(Guid groupId, CreateTenantGroupDto request);
    
    Task<Response<bool>> DeleteTenantGroupAsync(Guid groupId);
}