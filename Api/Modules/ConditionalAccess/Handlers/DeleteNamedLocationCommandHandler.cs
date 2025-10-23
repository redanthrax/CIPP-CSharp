using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class DeleteNamedLocationCommandHandler : IRequestHandler<DeleteNamedLocationCommand, Task> {
    private readonly INamedLocationService _locationService;

    public DeleteNamedLocationCommandHandler(INamedLocationService locationService) {
        _locationService = locationService;
    }

    public async Task Handle(DeleteNamedLocationCommand command, CancellationToken cancellationToken) {
        await _locationService.DeleteNamedLocationAsync(command.TenantId, command.LocationId, cancellationToken);
    }
}
