using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class AddDistributionGroupMemberCommandHandler : IRequestHandler<AddDistributionGroupMemberCommand, Task> {
    private readonly IDistributionGroupService _groupService;

    public AddDistributionGroupMemberCommandHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task Handle(AddDistributionGroupMemberCommand command, CancellationToken cancellationToken) {
        await _groupService.AddDistributionGroupMemberAsync(command.TenantId, command.GroupId, command.MemberEmail);
    }
}
