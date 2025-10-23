using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record DeleteIntuneAppCommand(string TenantId, string AppId) 
    : IRequest<DeleteIntuneAppCommand, Task>;
