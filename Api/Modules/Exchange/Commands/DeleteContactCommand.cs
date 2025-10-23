using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record DeleteContactCommand(string TenantId, string ContactId) : IRequest<DeleteContactCommand, Task>;
