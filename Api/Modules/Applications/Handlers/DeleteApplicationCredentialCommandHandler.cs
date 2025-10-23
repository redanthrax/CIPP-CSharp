using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DeleteApplicationCredentialCommandHandler : IRequestHandler<DeleteApplicationCredentialCommand, Task> {
    private readonly IApplicationService _applicationService;
    private readonly ILogger<DeleteApplicationCredentialCommandHandler> _logger;

    public DeleteApplicationCredentialCommandHandler(
        IApplicationService applicationService,
        ILogger<DeleteApplicationCredentialCommandHandler> logger) {
        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task Handle(DeleteApplicationCredentialCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Deleting credential {KeyId} for application {ApplicationId} in tenant {TenantId}", request.DeleteCredentialDto.KeyId, request.DeleteCredentialDto.ApplicationId, request.DeleteCredentialDto.TenantId);
            await _applicationService.DeleteApplicationCredentialAsync(request.DeleteCredentialDto, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete application credential");
            throw new InvalidOperationException($"Failed to delete application credential: {ex.Message}", ex);
        }
    }
}
