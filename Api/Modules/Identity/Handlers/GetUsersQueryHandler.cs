using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Task<PagedResponse<UserDto>>> {
    private readonly IUserService _userService;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUserService userService,
        ILogger<GetUsersQueryHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<PagedResponse<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Retrieving users for tenant {TenantId}", request.TenantId);
        var result = await _userService.GetUsersAsync(request.TenantId, request.Paging, cancellationToken);
        return result;
    }
}
