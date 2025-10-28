using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxAutoReplyConfigurationCommand(Guid TenantId, string MailboxId, UpdateMailboxAutoReplyConfigurationDto UpdateDto)
    : IRequest<UpdateMailboxAutoReplyConfigurationCommand, Task>;
