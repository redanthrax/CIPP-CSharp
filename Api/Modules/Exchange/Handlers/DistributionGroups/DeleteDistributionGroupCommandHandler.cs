using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class DeleteDistributionGroupCommandHandler : IRequestHandler<DeleteDistributionGroupCommand, Task> {
    private readonly IDistributionGroupService _groupService;

    public DeleteDistributionGroupCommandHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task Handle(DeleteDistributionGroupCommand command, CancellationToken cancellationToken) {
        await _groupService.DeleteDistributionGroupAsync(command.TenantId, command.GroupId);
    }
}
