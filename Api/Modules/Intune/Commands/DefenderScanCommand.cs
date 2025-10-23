using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record DefenderScanCommand(string TenantId, string DeviceId, bool QuickScan) 
    : IRequest<DefenderScanCommand, Task>;
