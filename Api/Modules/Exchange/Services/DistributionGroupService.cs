using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class DistributionGroupService : IDistributionGroupService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<DistributionGroupService> _logger;

    public DistributionGroupService(IExchangeOnlineService exoService, ILogger<DistributionGroupService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<DistributionGroupDto>> GetDistributionGroupsAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting distribution groups for tenant {TenantId}", tenantId);
        var groups = await _exoService.ExecuteCmdletListAsync<DistributionGroupDto>(tenantId, "Get-DistributionGroup", null, cancellationToken);
        return groups.ToPagedResponse(pagingParams);
    }

    public async Task<DistributionGroupDto?> GetDistributionGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", groupId } };
        return await _exoService.ExecuteCmdletAsync<DistributionGroupDto>(tenantId, "Get-DistributionGroup", parameters, cancellationToken);
    }

    public async Task CreateDistributionGroupAsync(string tenantId, CreateDistributionGroupDto createDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "PrimarySmtpAddress", createDto.PrimarySmtpAddress },
            { "RequireSenderAuthenticationEnabled", createDto.RequireSenderAuthenticationEnabled }
        };
        if (!string.IsNullOrEmpty(createDto.Description)) parameters.Add("Notes", createDto.Description);
        if (createDto.ManagedBy?.Any() == true) parameters.Add("ManagedBy", createDto.ManagedBy.ToArray());
        if (createDto.Members?.Any() == true) parameters.Add("Members", createDto.Members.ToArray());
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "New-DistributionGroup", parameters, cancellationToken);
    }

    public async Task UpdateDistributionGroupAsync(string tenantId, string groupId, UpdateDistributionGroupDto updateDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", groupId } };
        if (!string.IsNullOrEmpty(updateDto.DisplayName)) parameters.Add("DisplayName", updateDto.DisplayName);
        if (!string.IsNullOrEmpty(updateDto.Description)) parameters.Add("Notes", updateDto.Description);
        if (updateDto.ManagedBy?.Any() == true) parameters.Add("ManagedBy", updateDto.ManagedBy.ToArray());
        if (updateDto.RequireSenderAuthenticationEnabled.HasValue) parameters.Add("RequireSenderAuthenticationEnabled", updateDto.RequireSenderAuthenticationEnabled.Value);
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Set-DistributionGroup", parameters, cancellationToken);
    }

    public async Task DeleteDistributionGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", groupId } };
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Remove-DistributionGroup", parameters, cancellationToken);
    }

    public async Task<PagedResponse<DistributionGroupMemberDto>> GetDistributionGroupMembersAsync(string tenantId, string groupId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", groupId } };
        var members = await _exoService.ExecuteCmdletListAsync<DistributionGroupMemberDto>(tenantId, "Get-DistributionGroupMember", parameters, cancellationToken);
        return members.ToPagedResponse(pagingParams);
    }

    public async Task AddDistributionGroupMemberAsync(string tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Identity", groupId },
            { "Member", memberEmail }
        };
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Add-DistributionGroupMember", parameters, cancellationToken);
    }

    public async Task RemoveDistributionGroupMemberAsync(string tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Identity", groupId },
            { "Member", memberEmail }
        };
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Remove-DistributionGroupMember", parameters, cancellationToken);
    }
}
