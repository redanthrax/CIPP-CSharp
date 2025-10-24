using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.CalendarProcessing;

public record UpdateCalendarProcessingCommand(string TenantId, string MailboxIdentity, UpdateCalendarProcessingDto UpdateDto) : IRequest<UpdateCalendarProcessingCommand, Task>;
