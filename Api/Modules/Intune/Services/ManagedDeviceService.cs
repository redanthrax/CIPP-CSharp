using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.DeviceManagement.ManagedDevices.Item.Wipe;
using Microsoft.Graph.Beta.DeviceManagement.ManagedDevices.Item.CleanWindowsDevice;
using Microsoft.Graph.Beta.DeviceManagement.ManagedDevices.Item.WindowsDefenderScan;

namespace CIPP.Api.Modules.Intune.Services;

public class ManagedDeviceService : IManagedDeviceService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<ManagedDeviceService> _logger;

    public ManagedDeviceService(IMicrosoftGraphService graphService, ILogger<ManagedDeviceService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task WipeDeviceAsync(string tenantId, string deviceId, bool keepEnrollmentData, bool keepUserData, bool useProtectedWipe, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Wiping device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var wipeBody = new WipePostRequestBody {
            KeepEnrollmentData = keepEnrollmentData,
            KeepUserData = keepUserData,
            UseProtectedWipe = useProtectedWipe
        };

        await graphClient.DeviceManagement.ManagedDevices[deviceId].Wipe.PostAsync(wipeBody, cancellationToken: cancellationToken);
    }

    public async Task RetireDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Retiring device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].Retire.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].DeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task SyncDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Syncing device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].SyncDevice.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task RebootDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Rebooting device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].RebootNow.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task LocateDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Locating device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].LocateDevice.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task ResetPasscodeAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Resetting passcode for device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].ResetPasscode.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task RemoteLockDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Remote locking device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].RemoteLock.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task SetDeviceNameAsync(string tenantId, string deviceId, string deviceName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Setting device name for {DeviceId} to {DeviceName} for tenant {TenantId}", deviceId, deviceName, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var setDeviceNameBody = new Microsoft.Graph.Beta.DeviceManagement.ManagedDevices.Item.SetDeviceName.SetDeviceNamePostRequestBody {
            DeviceName = deviceName
        };

        await graphClient.DeviceManagement.ManagedDevices[deviceId].SetDeviceName.PostAsync(setDeviceNameBody, cancellationToken: cancellationToken);
    }

    public async Task RotateLocalAdminPasswordAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Rotating local admin password for device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].RotateLocalAdminPassword.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task DefenderScanAsync(string tenantId, string deviceId, bool quickScan, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Running {ScanType} Defender scan on device {DeviceId} for tenant {TenantId}", quickScan ? "quick" : "full", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var scanBody = new WindowsDefenderScanPostRequestBody {
            QuickScan = quickScan
        };

        await graphClient.DeviceManagement.ManagedDevices[deviceId].WindowsDefenderScan.PostAsync(scanBody, cancellationToken: cancellationToken);
    }

    public async Task DefenderUpdateSignaturesAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating Defender signatures for device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.DeviceManagement.ManagedDevices[deviceId].WindowsDefenderUpdateSignatures.PostAsync(cancellationToken: cancellationToken);
    }

    public async Task CreateDeviceLogCollectionAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating log collection request for device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var logCollectionBody = new Microsoft.Graph.Beta.DeviceManagement.ManagedDevices.Item.CreateDeviceLogCollectionRequest.CreateDeviceLogCollectionRequestPostRequestBody();
        await graphClient.DeviceManagement.ManagedDevices[deviceId].CreateDeviceLogCollectionRequest.PostAsync(logCollectionBody, cancellationToken: cancellationToken);
    }

    public async Task CleanWindowsDeviceAsync(string tenantId, string deviceId, bool keepUserData, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Cleaning Windows device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var cleanBody = new CleanWindowsDevicePostRequestBody {
            KeepUserData = keepUserData
        };

        await graphClient.DeviceManagement.ManagedDevices[deviceId].CleanWindowsDevice.PostAsync(cleanBody, cancellationToken: cancellationToken);
    }
}
