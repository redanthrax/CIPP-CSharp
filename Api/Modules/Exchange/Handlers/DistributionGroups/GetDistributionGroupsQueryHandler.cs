using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class GetDistributionGroupsQueryHandler : IRequestHandler<GetDistributionGroupsQuery, Task<PagedResponse<DistributionGroupDto>>> {
    private readonly IDistributionGroupService _groupService;

    public GetDistributionGroupsQueryHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task<PagedResponse<DistributionGroupDto>> Handle(GetDistributionGroupsQuery query, CancellationToken cancellationToken) {
        return await _groupService.GetDistributionGroupsAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}
