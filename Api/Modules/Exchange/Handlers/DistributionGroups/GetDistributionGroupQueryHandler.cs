using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class GetDistributionGroupQueryHandler : IRequestHandler<GetDistributionGroupQuery, Task<DistributionGroupDto?>> {
    private readonly IDistributionGroupService _groupService;

    public GetDistributionGroupQueryHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task<DistributionGroupDto?> Handle(GetDistributionGroupQuery query, CancellationToken cancellationToken) {
        return await _groupService.GetDistributionGroupAsync(query.TenantId, query.GroupId);
    }
}
