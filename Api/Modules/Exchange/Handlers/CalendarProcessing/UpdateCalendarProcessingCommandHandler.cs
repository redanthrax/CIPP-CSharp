using CIPP.Api.Modules.Exchange.Commands.CalendarProcessing;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.CalendarProcessing;

public class UpdateCalendarProcessingCommandHandler : IRequestHandler<UpdateCalendarProcessingCommand, Task> {
    private readonly ICalendarProcessingService _calendarProcessingService;

    public UpdateCalendarProcessingCommandHandler(ICalendarProcessingService calendarProcessingService) {
        _calendarProcessingService = calendarProcessingService;
    }

    public async Task Handle(UpdateCalendarProcessingCommand command, CancellationToken cancellationToken) {
        await _calendarProcessingService.UpdateCalendarProcessingAsync(command.TenantId, command.MailboxIdentity, command.UpdateDto, cancellationToken);
    }
}
