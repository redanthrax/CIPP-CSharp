using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetUserMfaStatusQueryHandler : IRequestHandler<GetUserMfaStatusQuery, Task<UserMfaStatusDto>> {
    private readonly IUserService _userService;
    private readonly ILogger<GetUserMfaStatusQueryHandler> _logger;

    public GetUserMfaStatusQueryHandler(
        IUserService userService,
        ILogger<GetUserMfaStatusQueryHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<UserMfaStatusDto> Handle(GetUserMfaStatusQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving MFA status for user {UserId} in tenant {TenantId}", 
                request.UserId, request.TenantId);
            
            return await _userService.GetUserMfaStatusAsync(request.TenantId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve MFA status for user {UserId}", request.UserId);
            return new UserMfaStatusDto {
                Enabled = false,
                Enforced = false,
                Methods = new List<string>(),
                DefaultMethod = ""
            };
        }
    }
}