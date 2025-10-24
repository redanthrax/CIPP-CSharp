using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class RemoveDistributionGroupMemberCommandHandler : IRequestHandler<RemoveDistributionGroupMemberCommand, Task> {
    private readonly IDistributionGroupService _groupService;

    public RemoveDistributionGroupMemberCommandHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task Handle(RemoveDistributionGroupMemberCommand command, CancellationToken cancellationToken) {
        await _groupService.RemoveDistributionGroupMemberAsync(command.TenantId, command.GroupId, command.MemberEmail);
    }
}
