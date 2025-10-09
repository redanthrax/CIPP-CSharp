using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Frontend.Modules.Tenants.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Frontend.Modules.Tenants.Services;

public class TenantService : ITenantService {
    private readonly ICippApiClient _apiClient;
    
    public TenantService(ICippApiClient apiClient) {
        _apiClient = apiClient;
    }
    
    public async Task<PagedResponse<TenantDto>> GetTenantsAsync(int pageNumber = 1, int pageSize = 50, 
        bool allTenants = false, bool includeOffboardingDefaults = false, bool noCache = false) {
        
        var queryParams = new List<string>();
        if (allTenants) queryParams.Add("AllTenantSelector=true");
        if (includeOffboardingDefaults) queryParams.Add("IncludeOffboardingDefaults=true");
        
        var endpoint = "tenants";
        if (queryParams.Any()) {
            endpoint += "?" + string.Join("&", queryParams);
        }
        
        return await _apiClient.GetPagedAsync<TenantDto>(endpoint, pageNumber, pageSize, noCache);
    }
    
    public async Task<Response<TenantDetailsDto>> GetTenantDetailsAsync(Guid tenantId) {
        return await _apiClient.GetAsync<TenantDetailsDto>($"tenants/{tenantId}/details");
    }
    
    public async Task<Response<List<TenantSelectorOptionDto>>> GetTenantSelectorOptionsAsync(
        bool allTenants = false, bool includeGroups = false, bool includeOffboardingDefaults = false) {
        
        try {
            var selectorOptions = new List<TenantSelectorOptionDto>();
            
            // Add "All Tenants" option if requested
            if (allTenants) {
                selectorOptions.Add(new TenantSelectorOptionDto(
                    "AllTenants",
                    "All Tenants",
                    "Group",
                    "",
                    "All Tenants",
                    Guid.Empty
                ));
            }
            
            // Get individual tenants
            var tenantsResponse = await GetTenantsAsync(pageSize: 1000, allTenants: true, includeOffboardingDefaults: includeOffboardingDefaults);
            if (tenantsResponse?.Items != null && tenantsResponse.Items.Any()) {
                selectorOptions.AddRange(tenantsResponse.Items
                    .Where(t => includeOffboardingDefaults || !t.Excluded)
                    .Select(t => new TenantSelectorOptionDto(
                        t.TenantId,
                        t.DisplayName,
                        "Tenant",
                        t.DefaultDomainName,
                        t.DisplayName,
                        t.Id,
                        t.Excluded ? "Excluded" : null
                    )));
            }
            
            // Add tenant groups if requested
            if (includeGroups) {
                var groupsResponse = await GetTenantGroupsAsync(pageSize: 1000);
                if (groupsResponse?.Items != null && groupsResponse.Items.Any()) {
                    selectorOptions.AddRange(groupsResponse.Items
                        .Select(g => new TenantSelectorOptionDto(
                            g.Id.ToString(),
                            g.Name,
                            "Group",
                            "",
                            g.Name,
                            g.Id
                        )));
                }
            }
            
            return Response<List<TenantSelectorOptionDto>>.SuccessResult(selectorOptions);
        } catch (Exception ex) {
            return Response<List<TenantSelectorOptionDto>>.ErrorResult($"Failed to load tenant selector options: {ex.Message}");
        }
    }
    
    public async Task<Response<ValidateDomainResponseDto>> ValidateTenantDomainAsync(string tenantName) {
        return await _apiClient.GetAsync<ValidateDomainResponseDto>($"tenants/validate-domain?tenantName={tenantName}");
    }
    
    public async Task<Response<TenantDto>> AddTenantAsync(AddTenantRequestDto request) {
        return await _apiClient.PostAsync<TenantDto>("tenants", request);
    }
    
    public async Task<Response<TenantDetailsDto>> UpdateTenantAsync(EditTenantRequestDto request) {
        return await _apiClient.PutAsync<TenantDetailsDto>($"tenants/{request.Id}", request);
    }
    
    public async Task<Response<bool>> DeleteTenantAsync(Guid tenantId) {
        return await _apiClient.DeleteAsync($"tenants/{tenantId}");
    }
    
    public async Task<Response<string>> ExcludeTenantAsync(List<Guid> tenantIds, bool addExclusion) {
        var request = new TenantExclusionDto(tenantIds, addExclusion);
        return await _apiClient.PostAsync<string>("tenants/exclude", request);
    }
    
    public async Task<PagedResponse<TenantGroupDto>> GetTenantGroupsAsync(int pageNumber = 1, int pageSize = 50, 
        bool noCache = false) {
        return await _apiClient.GetPagedAsync<TenantGroupDto>("tenants/groups", pageNumber, pageSize, noCache);
    }
    
    public async Task<Response<TenantGroupDto>> GetTenantGroupAsync(Guid groupId) {
        return await _apiClient.GetAsync<TenantGroupDto>($"tenants/groups/{groupId}");
    }
    
    public async Task<Response<TenantGroupDto>> AddTenantGroupAsync(CreateTenantGroupDto request) {
        return await _apiClient.PostAsync<TenantGroupDto>("tenants/groups", request);
    }
    
    public async Task<Response<TenantGroupDto>> UpdateTenantGroupAsync(Guid groupId, CreateTenantGroupDto request) {
        return await _apiClient.PutAsync<TenantGroupDto>($"tenants/groups/{groupId}", request);
    }
    
    public async Task<Response<bool>> DeleteTenantGroupAsync(Guid groupId) {
        return await _apiClient.DeleteAsync($"tenants/groups/{groupId}");
    }
}