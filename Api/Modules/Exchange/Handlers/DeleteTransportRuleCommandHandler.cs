using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class DeleteTransportRuleCommandHandler : IRequestHandler<DeleteTransportRuleCommand, Task> {
    private readonly ITransportRuleService _transportRuleService;

    public DeleteTransportRuleCommandHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task Handle(DeleteTransportRuleCommand command, CancellationToken cancellationToken) {
        await _transportRuleService.DeleteTransportRuleAsync(command.TenantId, command.RuleId, cancellationToken);
    }
}
