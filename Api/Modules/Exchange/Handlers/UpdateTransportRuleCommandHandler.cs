using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateTransportRuleCommandHandler : IRequestHandler<UpdateTransportRuleCommand, Task> {
    private readonly ITransportRuleService _transportRuleService;

    public UpdateTransportRuleCommandHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task Handle(UpdateTransportRuleCommand command, CancellationToken cancellationToken) {
        await _transportRuleService.UpdateTransportRuleAsync(command.TenantId, command.RuleId, command.UpdateDto, cancellationToken);
    }
}
