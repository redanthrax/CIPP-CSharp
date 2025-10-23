using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand, Task<string>> {
    private readonly IUserService _userService;
    private readonly ILogger<ResetUserPasswordCommandHandler> _logger;

    public ResetUserPasswordCommandHandler(
        IUserService userService,
        ILogger<ResetUserPasswordCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<string> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Resetting password for user {UserId} in tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            return await _userService.ResetUserPasswordAsync(request.TenantId, request.UserId, request.PasswordData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to reset password for user {UserId}", request.UserId);
            throw new InvalidOperationException($"Failed to reset user password: {ex.Message}", ex);
        }
    }
}