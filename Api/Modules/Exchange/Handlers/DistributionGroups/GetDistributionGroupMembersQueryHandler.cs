using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class GetDistributionGroupMembersQueryHandler : IRequestHandler<GetDistributionGroupMembersQuery, Task<PagedResponse<DistributionGroupMemberDto>>> {
    private readonly IDistributionGroupService _groupService;

    public GetDistributionGroupMembersQueryHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task<PagedResponse<DistributionGroupMemberDto>> Handle(GetDistributionGroupMembersQuery query, CancellationToken cancellationToken) {
        return await _groupService.GetDistributionGroupMembersAsync(query.TenantId, query.GroupId, query.PagingParams, cancellationToken);
    }
}
