using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Commands;

public record SyncTenantFromGraphCommand(Guid TenantId) : IRequest<SyncTenantFromGraphCommand, Task<string>>;
