using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class CreateApplicationCredentialCommandHandler : IRequestHandler<CreateApplicationCredentialCommand, Task<ApplicationCredentialDto>> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<CreateApplicationCredentialCommandHandler> _logger;

    public CreateApplicationCredentialCommandHandler(
        IApplicationService applicationService,
        ILogger<CreateApplicationCredentialCommandHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<ApplicationCredentialDto> Handle(CreateApplicationCredentialCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Creating credential for application {ApplicationId} in tenant {TenantId}", request.CreateCredentialDto.ApplicationId, request.CreateCredentialDto.TenantId);
            return await _applicationService.CreateApplicationCredentialAsync(request.CreateCredentialDto, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create application credential");
            throw new InvalidOperationException($"Failed to create application credential: {ex.Message}", ex);
        }
    }
}
