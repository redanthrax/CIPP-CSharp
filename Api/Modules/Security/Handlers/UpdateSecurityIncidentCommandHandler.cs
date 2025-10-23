using CIPP.Api.Modules.Security.Commands;
using CIPP.Api.Modules.Security.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class UpdateSecurityIncidentCommandHandler : IRequestHandler<UpdateSecurityIncidentCommand, Task> {
    private readonly ISecurityIncidentService _incidentService;

    public UpdateSecurityIncidentCommandHandler(ISecurityIncidentService incidentService) {
        _incidentService = incidentService;
    }

    public async Task Handle(UpdateSecurityIncidentCommand command, CancellationToken cancellationToken) {
        await _incidentService.UpdateIncidentAsync(command.TenantId, command.IncidentId, command.UpdateDto, cancellationToken);
    }
}
