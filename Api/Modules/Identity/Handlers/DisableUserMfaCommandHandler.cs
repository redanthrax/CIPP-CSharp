using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class DisableUserMfaCommandHandler : IRequestHandler<DisableUserMfaCommand, Task> {
    private readonly IUserService _userService;
    private readonly ILogger<DisableUserMfaCommandHandler> _logger;

    public DisableUserMfaCommandHandler(
        IUserService userService,
        ILogger<DisableUserMfaCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task Handle(DisableUserMfaCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Disabling MFA for user {UserId} in tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            await _userService.DisableUserMfaAsync(request.TenantId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to disable MFA for user {UserId}", request.UserId);
            throw new InvalidOperationException($"Failed to disable user MFA: {ex.Message}", ex);
        }
    }
}