using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Task<UserDto>> {
    private readonly IUserService _userService;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserService userService,
        ILogger<UpdateUserCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Updating user {UserId} for tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            return await _userService.UpdateUserAsync(request.TenantId, request.UserId, request.UserData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to update user {UserId}", request.UserId);
            throw new InvalidOperationException($"Failed to update user: {ex.Message}", ex);
        }
    }
}