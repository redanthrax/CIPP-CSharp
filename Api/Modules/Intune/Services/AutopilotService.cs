using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Intune;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Intune.Services;

public class AutopilotService : IAutopilotService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<AutopilotService> _logger;

    public AutopilotService(IMicrosoftGraphService graphService, ILogger<AutopilotService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<AutopilotDeviceDto>> GetDevicesAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Autopilot devices for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var devices = await graphClient.DeviceManagement.WindowsAutopilotDeviceIdentities.GetAsync(config => {
            config.QueryParameters.Top = 999;
        }, cancellationToken);

        if (devices?.Value == null) {
            return new List<AutopilotDeviceDto>();
        }

        return devices.Value.Select(d => MapToDeviceDto(d, tenantId)).ToList();
    }

    public async Task<AutopilotDeviceDto?> GetDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Autopilot device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var device = await graphClient.DeviceManagement.WindowsAutopilotDeviceIdentities[deviceId].GetAsync(cancellationToken: cancellationToken);

        return device != null ? MapToDeviceDto(device, tenantId) : null;
    }

    public async Task DeleteDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting Autopilot device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.WindowsAutopilotDeviceIdentities[deviceId].DeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task SyncDevicesAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Syncing Autopilot devices for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.WindowsAutopilotSettings.Sync.PostAsync(cancellationToken: cancellationToken);
    }

    private static AutopilotDeviceDto MapToDeviceDto(WindowsAutopilotDeviceIdentity device, Guid tenantId) {
        return new AutopilotDeviceDto {
            Id = device.Id ?? string.Empty,
            SerialNumber = device.SerialNumber,
            Model = device.Model,
            Manufacturer = device.Manufacturer,
            ProductKey = device.ProductKey,
            GroupTag = device.GroupTag,
            UserPrincipalName = device.UserPrincipalName,
            AddressableUserName = device.AddressableUserName,
            AzureActiveDirectoryDeviceId = device.AzureAdDeviceId,
            ManagedDeviceId = device.ManagedDeviceId,
            DisplayName = device.DisplayName,
            LastContactedDateTime = device.LastContactedDateTime?.DateTime,
            TenantId = tenantId
        };
    }
}
