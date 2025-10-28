using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateMailboxCalendarConfigurationCommand(Guid TenantId, string MailboxId, UpdateMailboxCalendarConfigurationDto UpdateDto)
    : IRequest<UpdateMailboxCalendarConfigurationCommand, Task>;
