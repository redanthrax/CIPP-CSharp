using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxQuotaCommand(string TenantId, string MailboxId, UpdateMailboxQuotaDto UpdateDto) : IRequest<UpdateMailboxQuotaCommand, Task>;
