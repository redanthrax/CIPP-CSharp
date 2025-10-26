namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IManagedDeviceService {
    Task WipeDeviceAsync(Guid tenantId, string deviceId, bool keepEnrollmentData, bool keepUserData, bool useProtectedWipe, CancellationToken cancellationToken = default);
    Task RetireDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SyncDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task RebootDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task LocateDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task ResetPasscodeAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task RemoteLockDeviceAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SetDeviceNameAsync(Guid tenantId, string deviceId, string deviceName, CancellationToken cancellationToken = default);
    Task RotateLocalAdminPasswordAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DefenderScanAsync(Guid tenantId, string deviceId, bool quickScan, CancellationToken cancellationToken = default);
    Task DefenderUpdateSignaturesAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task CreateDeviceLogCollectionAsync(Guid tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task CleanWindowsDeviceAsync(Guid tenantId, string deviceId, bool keepUserData, CancellationToken cancellationToken = default);
}
