using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class CreateBulkUsersCommandHandler : IRequestHandler<CreateBulkUsersCommand, Task<List<BulkUserResultDto>>> {
    private readonly IUserService _userService;
    private readonly ILogger<CreateBulkUsersCommandHandler> _logger;

    public CreateBulkUsersCommandHandler(
        IUserService userService,
        ILogger<CreateBulkUsersCommandHandler> logger) {
        _userService = userService;
        _logger = logger;
    }

    public async Task<List<BulkUserResultDto>> Handle(CreateBulkUsersCommand request, CancellationToken cancellationToken) {
        var results = new List<BulkUserResultDto>();

        _logger.LogInformation("Processing bulk user creation for {UserCount} users in tenant {TenantId}",
            request.BulkUserData.BulkUser.Count, request.BulkUserData.TenantFilter);

        foreach (var user in request.BulkUserData.BulkUser) {
            try {
                if (string.IsNullOrEmpty(user.TenantId)) {
                    user.TenantId = request.BulkUserData.TenantFilter;
                }

                if (string.IsNullOrEmpty(user.Country) && !string.IsNullOrEmpty(request.BulkUserData.UsageLocation)) {
                    user.Country = request.BulkUserData.UsageLocation;
                }

                if (!user.AssignedLicenses.Any() && request.BulkUserData.Licenses.Any()) {
                    user.AssignedLicenses.AddRange(request.BulkUserData.Licenses);
                }

                var createdUser = await _userService.CreateUserAsync(user, cancellationToken);
                results.Add(new BulkUserResultDto {
                    ResultText = $"Created user {createdUser.DisplayName} with username {createdUser.UserPrincipalName}",
                    State = "success",
                    CopyField = user.Password, // Return the password that was used
                    Username = createdUser.UserPrincipalName
                });

            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to create user {UserPrincipalName}", user.UserPrincipalName);
                
                results.Add(new BulkUserResultDto {
                    ResultText = $"Failed to create user {user.UserPrincipalName ?? user.DisplayName}. Error: {ex.Message}",
                    State = "error"
                });
            }
        }

        return results;
    }
}