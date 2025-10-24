using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.DistributionGroups;

public class CreateDistributionGroupCommandHandler : IRequestHandler<CreateDistributionGroupCommand, Task> {
    private readonly IDistributionGroupService _groupService;

    public CreateDistributionGroupCommandHandler(IDistributionGroupService groupService) {
        _groupService = groupService;
    }

    public async Task Handle(CreateDistributionGroupCommand command, CancellationToken cancellationToken) {
        await _groupService.CreateDistributionGroupAsync(command.TenantId, command.Group);
    }
}
