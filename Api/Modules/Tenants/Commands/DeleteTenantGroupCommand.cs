using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Commands;

public record DeleteTenantGroupCommand(
    Guid GroupId
) : IRequest<DeleteTenantGroupCommand, Task<bool>>;
