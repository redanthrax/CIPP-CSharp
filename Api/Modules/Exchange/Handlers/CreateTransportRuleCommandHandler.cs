using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class CreateTransportRuleCommandHandler : IRequestHandler<CreateTransportRuleCommand, Task<string>> {
    private readonly ITransportRuleService _transportRuleService;

    public CreateTransportRuleCommandHandler(ITransportRuleService transportRuleService) {
        _transportRuleService = transportRuleService;
    }

    public async Task<string> Handle(CreateTransportRuleCommand command, CancellationToken cancellationToken) {
        return await _transportRuleService.CreateTransportRuleAsync(command.TenantId, command.CreateDto, cancellationToken);
    }
}
