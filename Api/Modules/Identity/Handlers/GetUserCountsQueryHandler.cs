using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetUserCountsQueryHandler : IRequestHandler<GetUserCountsQuery, Task<UserCountsDto?>> {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<GetUserCountsQueryHandler> _logger;

    public GetUserCountsQueryHandler(
        IMicrosoftGraphService graphService,
        ILogger<GetUserCountsQueryHandler> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<UserCountsDto?> Handle(GetUserCountsQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving user counts for tenant {TenantId}", request.TenantId);

            var graphClient = await _graphService.GetGraphServiceClientAsync(request.TenantId);
            
            var userCounts = new UserCountsDto();

            try {
                var usersCount = await graphClient.Users.Count.GetAsync(config => {
                    config.Headers.Add("ConsistencyLevel", "eventual");
                }, cancellationToken);
                userCounts.Users = usersCount ?? 0;
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Failed to get total user count");
            }

            try {
                var licensedUsersResponse = await graphClient.Users.GetAsync(config => {
                    config.QueryParameters.Count = true;
                    config.QueryParameters.Top = 1;
                    config.QueryParameters.Filter = "assignedLicenses/$count ne 0";
                    config.Headers.Add("ConsistencyLevel", "eventual");
                }, cancellationToken);
                userCounts.LicUsers = (int)(licensedUsersResponse?.OdataCount ?? 0);
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Failed to get licensed user count");
            }

            try {
                var guestsResponse = await graphClient.Users.GetAsync(config => {
                    config.QueryParameters.Count = true;
                    config.QueryParameters.Top = 1;
                    config.QueryParameters.Filter = "userType eq 'Guest'";
                    config.Headers.Add("ConsistencyLevel", "eventual");
                }, cancellationToken);
                userCounts.Guests = (int)(guestsResponse?.OdataCount ?? 0);
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Failed to get guest user count");
            }

            try {
                var globalAdminsRole = await graphClient.DirectoryRoles["62e90394-69f5-4237-9190-012177145e10"].Members.GetAsync(cancellationToken: cancellationToken);
                userCounts.Gas = globalAdminsRole?.Value?.Count ?? 0;
            } catch (Exception ex) {
                _logger.LogWarning(ex, "Failed to get global admin count");
            }

            return userCounts;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve user counts for tenant {TenantId}", request.TenantId);
            return null;
        }
    }
}
