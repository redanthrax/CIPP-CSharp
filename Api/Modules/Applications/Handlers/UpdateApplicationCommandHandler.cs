using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand, Task<ApplicationDto>> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<UpdateApplicationCommandHandler> _logger;

    public UpdateApplicationCommandHandler(
        IApplicationService applicationService,
        ILogger<UpdateApplicationCommandHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<ApplicationDto> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Updating application {ApplicationId} for tenant {TenantId}", request.ApplicationId, request.TenantId);
        return await _applicationService.UpdateApplicationAsync(request.TenantId, request.ApplicationId, request.Application, cancellationToken);
    }
}
