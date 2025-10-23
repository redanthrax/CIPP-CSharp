using CIPP.Api.Modules.Security.Commands;
using CIPP.Api.Modules.Security.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class UpdateSecurityAlertCommandHandler : IRequestHandler<UpdateSecurityAlertCommand, Task> {
    private readonly ISecurityAlertService _alertService;

    public UpdateSecurityAlertCommandHandler(ISecurityAlertService alertService) {
        _alertService = alertService;
    }

    public async Task Handle(UpdateSecurityAlertCommand command, CancellationToken cancellationToken) {
        await _alertService.UpdateAlertAsync(command.TenantId, command.AlertId, command.UpdateDto, cancellationToken);
    }
}
