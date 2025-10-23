using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class EnableUserMfaCommandHandler : IRequestHandler<EnableUserMfaCommand, Task> {
    private readonly IUserService _userService;
    private readonly ILogger<EnableUserMfaCommandHandler> _logger;

    public EnableUserMfaCommandHandler(
        IUserService userService,
        ILogger<EnableUserMfaCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task Handle(EnableUserMfaCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Enabling MFA for user {UserId} in tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            await _userService.EnableUserMfaAsync(request.TenantId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to enable MFA for user {UserId}", request.UserId);
            throw new InvalidOperationException($"Failed to enable user MFA: {ex.Message}", ex);
        }
    }
}