using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class UpdateDistributionGroupCommandHandler : IRequestHandler<UpdateDistributionGroupCommand, Task> {
    private readonly IDistributionGroupService _groupService;

    public UpdateDistributionGroupCommandHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task Handle(UpdateDistributionGroupCommand command, CancellationToken cancellationToken) {
        await _groupService.UpdateDistributionGroupAsync(command.TenantId, command.GroupId, command.Group);
    }
}
