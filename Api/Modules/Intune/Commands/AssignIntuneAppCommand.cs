using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record AssignIntuneAppCommand(string TenantId, string AppId, string AssignTo) 
    : IRequest<AssignIntuneAppCommand, Task>;
