using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Task> {
    private readonly IUserService _userService;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserService userService,
        ILogger<DeleteUserCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Deleting user {UserId} for tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            await _userService.DeleteUserAsync(request.TenantId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete user {UserId}", request.UserId);
            throw new InvalidOperationException($"Failed to delete user: {ex.Message}", ex);
        }
    }
}