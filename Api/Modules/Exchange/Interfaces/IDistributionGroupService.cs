using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IDistributionGroupService {
    Task<PagedResponse<DistributionGroupDto>> GetDistributionGroupsAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<DistributionGroupDto?> GetDistributionGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
    Task CreateDistributionGroupAsync(string tenantId, CreateDistributionGroupDto createDto, CancellationToken cancellationToken = default);
    Task UpdateDistributionGroupAsync(string tenantId, string groupId, UpdateDistributionGroupDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteDistributionGroupAsync(string tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<PagedResponse<DistributionGroupMemberDto>> GetDistributionGroupMembersAsync(string tenantId, string groupId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task AddDistributionGroupMemberAsync(string tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default);
    Task RemoveDistributionGroupMemberAsync(string tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default);
}
