using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record DeleteGroupCommand(
    string TenantId,
    string GroupId
) : IRequest<DeleteGroupCommand, Task>;