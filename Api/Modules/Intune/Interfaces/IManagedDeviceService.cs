namespace CIPP.Api.Modules.Intune.Interfaces;

public interface IManagedDeviceService {
    Task WipeDeviceAsync(string tenantId, string deviceId, bool keepEnrollmentData, bool keepUserData, bool useProtectedWipe, CancellationToken cancellationToken = default);
    Task RetireDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DeleteDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SyncDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task RebootDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task LocateDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task ResetPasscodeAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task RemoteLockDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task SetDeviceNameAsync(string tenantId, string deviceId, string deviceName, CancellationToken cancellationToken = default);
    Task RotateLocalAdminPasswordAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task DefenderScanAsync(string tenantId, string deviceId, bool quickScan, CancellationToken cancellationToken = default);
    Task DefenderUpdateSignaturesAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task CreateDeviceLogCollectionAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task CleanWindowsDeviceAsync(string tenantId, string deviceId, bool keepUserData, CancellationToken cancellationToken = default);
}
