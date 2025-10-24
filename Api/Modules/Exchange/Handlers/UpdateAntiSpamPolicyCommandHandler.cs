using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateAntiSpamPolicyCommandHandler : IRequestHandler<UpdateAntiSpamPolicyCommand, Task> {
    private readonly ISpamFilterService _spamFilterService;

    public UpdateAntiSpamPolicyCommandHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task Handle(UpdateAntiSpamPolicyCommand command, CancellationToken cancellationToken) {
        await _spamFilterService.UpdateAntiSpamPolicyAsync(command.TenantId, command.PolicyId, command.UpdateDto, cancellationToken);
    }
}
