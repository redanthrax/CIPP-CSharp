using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class CreateNamedLocationCommandHandler : IRequestHandler<CreateNamedLocationCommand, Task<NamedLocationDto>> {
    private readonly INamedLocationService _locationService;

    public CreateNamedLocationCommandHandler(INamedLocationService locationService) {
        _locationService = locationService;
    }

    public async Task<NamedLocationDto> Handle(CreateNamedLocationCommand command, CancellationToken cancellationToken) {
        return await _locationService.CreateNamedLocationAsync(command.Location, cancellationToken);
    }
}
