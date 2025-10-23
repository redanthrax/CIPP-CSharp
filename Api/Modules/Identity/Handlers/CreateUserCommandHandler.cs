using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Task<UserDto>> {
    private readonly IUserService _userService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUserService userService,
        ILogger<CreateUserCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Creating user {UserPrincipalName} for tenant {TenantId}", 
                request.UserData.UserPrincipalName, request.UserData.TenantId);
            
            return await _userService.CreateUserAsync(request.UserData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create user {UserPrincipalName}", request.UserData.UserPrincipalName);
            throw new InvalidOperationException($"Failed to create user: {ex.Message}", ex);
        }
    }
}