using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Intune;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.DeviceAppManagement.MobileApps.Item.Assign;

namespace CIPP.Api.Modules.Intune.Services;

public class IntuneAppService : IIntuneAppService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<IntuneAppService> _logger;

    public IntuneAppService(IMicrosoftGraphService graphService, ILogger<IntuneAppService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<IntuneAppDto>> GetAppsAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Intune apps for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var apps = await graphClient.DeviceAppManagement.MobileApps.GetAsync(config => {
            config.QueryParameters.Top = 999;
            config.QueryParameters.Filter = "(microsoft.graph.managedApp/appAvailability eq null or microsoft.graph.managedApp/appAvailability eq 'lineOfBusiness' or isAssigned eq true)";
            config.QueryParameters.Orderby = new[] { "displayName" };
        }, cancellationToken);

        if (apps?.Value == null) {
            return new List<IntuneAppDto>();
        }

        return apps.Value.Select(app => MapToAppDto(app, tenantId)).ToList();
    }

    public async Task<IntuneAppDto?> GetAppAsync(Guid tenantId, string appId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Intune app {AppId} for tenant {TenantId}", appId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var app = await graphClient.DeviceAppManagement.MobileApps[appId].GetAsync(cancellationToken: cancellationToken);

        return app != null ? MapToAppDto(app, tenantId) : null;
    }

    public async Task AssignAppAsync(Guid tenantId, string appId, string assignTo, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Assigning Intune app {AppId} to {AssignTo} for tenant {TenantId}", appId, assignTo, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var assignBody = new AssignPostRequestBody {
            MobileAppAssignments = CreateAssignments(assignTo)
        };

        await graphClient.DeviceAppManagement.MobileApps[appId].Assign.PostAsync(assignBody, cancellationToken: cancellationToken);
    }

    public async Task DeleteAppAsync(Guid tenantId, string appId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting Intune app {AppId} for tenant {TenantId}", appId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceAppManagement.MobileApps[appId].DeleteAsync(cancellationToken: cancellationToken);
    }

    private static IntuneAppDto MapToAppDto(MobileApp app, Guid tenantId) {
        return new IntuneAppDto {
            Id = app.Id ?? string.Empty,
            DisplayName = app.DisplayName ?? string.Empty,
            Description = app.Description,
            Publisher = app.Publisher,
            CreatedDateTime = app.CreatedDateTime?.DateTime,
            LastModifiedDateTime = app.LastModifiedDateTime?.DateTime,
            AppType = app.OdataType?.Replace("#microsoft.graph.", ""),
            IsAssigned = app.IsAssigned ?? false,
            TenantId = tenantId
        };
    }

    private static List<MobileAppAssignment> CreateAssignments(string assignTo) {
        var assignments = new List<MobileAppAssignment>();

        switch (assignTo) {
            case "AllUsers":
                assignments.Add(new MobileAppAssignment {
                    Target = new AllLicensedUsersAssignmentTarget(),
                    Intent = InstallIntent.Required
                });
                break;
            case "AllDevices":
                assignments.Add(new MobileAppAssignment {
                    Target = new AllDevicesAssignmentTarget(),
                    Intent = InstallIntent.Required
                });
                break;
            case "Both":
                assignments.Add(new MobileAppAssignment {
                    Target = new AllLicensedUsersAssignmentTarget(),
                    Intent = InstallIntent.Required
                });
                assignments.Add(new MobileAppAssignment {
                    Target = new AllDevicesAssignmentTarget(),
                    Intent = InstallIntent.Required
                });
                break;
        }

        return assignments;
    }
}
