using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Frontend.Modules.Tenants.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;

namespace CIPP.Frontend.Modules.Tenants.Services;

public class TenantService : ITenantService {
    private readonly ICippApiClient _apiClient;
    private TenantDto? _selectedTenant;
    
    public event Action? OnSelectedTenantChanged;
    
    public TenantDto? SelectedTenant {
        get => _selectedTenant;
        set {
            if (_selectedTenant?.TenantId != value?.TenantId) {
                _selectedTenant = value;
                OnSelectedTenantChanged?.Invoke();
            }
        }
    }
    
    public Guid? SelectedTenantId => _selectedTenant?.TenantId;
    
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
        bool allTenants = false, bool includeGroups = false, bool includeOffboardingDefaults = false, int pageNumber = 1, bool noCache = false) {
        
        try {
            var selectorOptions = new List<TenantSelectorOptionDto>();
            
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
            
            if (pageNumber == 0) {
                var allTenantsList = new List<TenantDto>();
                var currentPage = 1;
                PagedResponse<TenantDto>? tenantsResponse;
                
                do {
                    tenantsResponse = await GetTenantsAsync(
                        pageNumber: currentPage, 
                        pageSize: 100, 
                        allTenants: true, 
                        includeOffboardingDefaults: includeOffboardingDefaults,
                        noCache: noCache);
                    
                    if (tenantsResponse?.Items != null && tenantsResponse.Items.Any()) {
                        allTenantsList.AddRange(tenantsResponse.Items);
                    }
                    currentPage++;
                } while (tenantsResponse?.Items?.Any() == true && tenantsResponse.Items.Count == 100);
                
                if (allTenantsList.Any()) {
                    selectorOptions.AddRange(allTenantsList
                        .Where(t => includeOffboardingDefaults || !t.Excluded)
                        .Select(t => new TenantSelectorOptionDto(
                            t.TenantId.ToString(),
                            t.DisplayName,
                            "Tenant",
                            t.DefaultDomainName,
                            t.DisplayName,
                            t.TenantId,
                            t.Excluded ? "Excluded" : null
                        )));
                }
            } else {
                var tenantsResponse = await GetTenantsAsync(
                    pageNumber: pageNumber, 
                    pageSize: 10, 
                    allTenants: true, 
                    includeOffboardingDefaults: includeOffboardingDefaults,
                    noCache: noCache);
                
                if (tenantsResponse?.Items != null && tenantsResponse.Items.Any()) {
                    selectorOptions.AddRange(tenantsResponse.Items
                        .Where(t => includeOffboardingDefaults || !t.Excluded)
                        .Select(t => new TenantSelectorOptionDto(
                            t.TenantId.ToString(),
                            t.DisplayName,
                            "Tenant",
                            t.DefaultDomainName,
                            t.DisplayName,
                            t.TenantId,
                            t.Excluded ? "Excluded" : null
                        )));
                }
            }
            
            if (includeGroups) {
                var groupsPageSize = pageNumber == 0 ? 100 : 10;
                if (pageNumber == 0) {
                    var allGroupsList = new List<TenantGroupDto>();
                    var currentPage = 1;
                    PagedResponse<TenantGroupDto>? groupsResponse;
                    
                    do {
                        groupsResponse = await GetTenantGroupsAsync(
                            pageNumber: currentPage, 
                            pageSize: groupsPageSize,
                            noCache: noCache);
                        
                        if (groupsResponse?.Items != null && groupsResponse.Items.Any()) {
                            allGroupsList.AddRange(groupsResponse.Items);
                        }
                        currentPage++;
                    } while (groupsResponse?.Items?.Any() == true && groupsResponse.Items.Count == groupsPageSize);
                    
                    if (allGroupsList.Any()) {
                        selectorOptions.AddRange(allGroupsList
                            .Select(g => new TenantSelectorOptionDto(
                                g.Id.ToString(),
                                g.Name,
                                "Group",
                                "",
                                g.Name,
                                g.Id
                            )));
                    }
                } else {
                    var groupsResponse = await GetTenantGroupsAsync(
                        pageNumber: pageNumber, 
                        pageSize: groupsPageSize,
                        noCache: noCache);
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
        return await _apiClient.PutAsync<TenantDetailsDto>($"tenants/{request.TenantId}", request);
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