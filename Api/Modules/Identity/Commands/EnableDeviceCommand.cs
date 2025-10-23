using DispatchR.Abstractions.Send; namespace CIPP.Api.Modules.Identity.Commands; public record EnableDeviceCommand(string TenantId, string DeviceId) : IRequest<EnableDeviceCommand, Task>;
