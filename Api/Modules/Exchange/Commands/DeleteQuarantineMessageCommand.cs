using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record DeleteQuarantineMessageCommand(string TenantId, string MessageId) : IRequest<DeleteQuarantineMessageCommand, Task>;
