using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Task> {
    private readonly IContactService _contactService;

    public UpdateContactCommandHandler(IContactService contactService) {
        _contactService = contactService;
    }

    public async Task Handle(UpdateContactCommand command, CancellationToken cancellationToken) {
        await _contactService.UpdateContactAsync(command.TenantId, command.ContactId, command.UpdateDto, cancellationToken);
    }
}
