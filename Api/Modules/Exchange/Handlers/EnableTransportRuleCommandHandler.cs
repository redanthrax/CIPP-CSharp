using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class EnableTransportRuleCommandHandler : IRequestHandler<EnableTransportRuleCommand, Task> {
    private readonly ITransportRuleService _transportRuleService;

    public EnableTransportRuleCommandHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task Handle(EnableTransportRuleCommand command, CancellationToken cancellationToken) {
        await _transportRuleService.EnableTransportRuleAsync(command.TenantId, command.RuleId, command.Enable, cancellationToken);
    }
}
