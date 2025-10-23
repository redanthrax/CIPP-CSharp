using CIPP.Api.Modules.Security.Commands;
using CIPP.Api.Modules.Security.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class UpdateSecureScoreControlCommandHandler : IRequestHandler<UpdateSecureScoreControlCommand, Task> {
    private readonly ISecureScoreService _secureScoreService;

    public UpdateSecureScoreControlCommandHandler(ISecureScoreService secureScoreService) {
        _secureScoreService = secureScoreService;
    }

    public async Task Handle(UpdateSecureScoreControlCommand command, CancellationToken cancellationToken) {
        await _secureScoreService.UpdateControlProfileAsync(command.TenantId, command.ControlName, command.UpdateDto, cancellationToken);
    }
}
