using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IDistributionGroupService {
    Task<PagedResponse<DistributionGroupDto>> GetDistributionGroupsAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<DistributionGroupDto?> GetDistributionGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
    Task CreateDistributionGroupAsync(Guid tenantId, CreateDistributionGroupDto createDto, CancellationToken cancellationToken = default);
    Task UpdateDistributionGroupAsync(Guid tenantId, string groupId, UpdateDistributionGroupDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteDistributionGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default);
    Task<PagedResponse<DistributionGroupMemberDto>> GetDistributionGroupMembersAsync(Guid tenantId, string groupId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task AddDistributionGroupMemberAsync(Guid tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default);
    Task RemoveDistributionGroupMemberAsync(Guid tenantId, string groupId, string memberEmail, CancellationToken cancellationToken = default);
}
