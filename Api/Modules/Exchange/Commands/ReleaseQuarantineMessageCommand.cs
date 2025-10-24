using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record ReleaseQuarantineMessageCommand(string TenantId, string MessageId) : IRequest<ReleaseQuarantineMessageCommand, Task>;
