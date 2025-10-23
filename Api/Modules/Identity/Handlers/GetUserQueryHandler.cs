using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Task<UserDto?>> {
    private readonly IUserService _userService;
    private readonly ILogger<GetUserQueryHandler> _logger;

    public GetUserQueryHandler(
        IUserService userService,
        ILogger<GetUserQueryHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving user {UserId} for tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            return await _userService.GetUserAsync(request.TenantId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve user {UserId} for tenant {TenantId}", 
                request.UserId, request.TenantId);
            return null;
        }
    }
}